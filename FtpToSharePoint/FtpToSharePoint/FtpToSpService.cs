// If compiling from the command line, compile with: /doc:FtpToSharePoint.xml
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;

using System.Xml.Schema;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

using System.Timers;

namespace FtpToSharePoint
{
    /// <summary>
    /// FtpToSpService is a Windows service that monitors a directory for new files(assumed to be XMLs). These files are validated agianst an XSD and added to a SharePoint list depending on validity. Each file is then moved into the archive subdirectory
    /// </summary>
    public partial class FtpToSpService : ServiceBase
    {
        List<String> _createdItems;
        string inbox = ConfigurationManager.AppSettings["inbox"];      //folder to xmls
        string archive = ConfigurationManager.AppSettings["archive"];  // folder to move processed xmls
        string schema = ConfigurationManager.AppSettings["schema"];    //schema to validate xmls against

        /// <summary>
        /// Initializes FtpToSpService to monitor <c>AppSettings["inbox"]</c>
        /// </summary>
        public FtpToSpService()
        {
            InitializeComponent();
            _createdItems = new List<string>();
            _ftpWatcher.Path = Path.GetDirectoryName(inbox);    //give directory to be watched
        }
        
        /// <summary>
        /// Called when service starts. Allows _ftpWatcher to raise events
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Called when service is first started.
            base.OnStart(args);
            _ftpWatcher.EnableRaisingEvents = true;

            System.Timers.Timer heartbeat = new System.Timers.Timer();  //heartbeat timer
            heartbeat.Elapsed += new ElapsedEventHandler(beat);         // "beat" at each interval
            heartbeat.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["HeartbeatInterval"]);   //use config to set interval
            heartbeat.Enabled = true;   //start heartbeat
        }


        /// <summary>
        /// Creates a heartbeat file with a timestamp to let you know if the service is still alive.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void beat(object source, ElapsedEventArgs e)
        {
            string heartbeatPath = ConfigurationManager.AppSettings["Heartbeat"];   //heartbeat file location
            if (System.IO.File.Exists(heartbeatPath))System.IO.File.Delete(heartbeatPath); 
            using (FileStream fs = System.IO.File.Create(heartbeatPath, 1024))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("Yes, I'm still alive! (As of : " + DateTime.Now + ").");
                fs.Write(info, 0, info.Length); //add heartbeat content
            }
        }

        /// <summary>
        /// Called when service is paused. 
        /// </summary>
        protected override void OnPause()
        {
            _ftpWatcher.EnableRaisingEvents = false;
            base.OnPause();
            LogEntry logEntryStart = new LogEntry { Message = "ftp2sp service paused", Severity = TraceEventType.Information };
            Logger.Write(logEntryStart);
        }

        /// <summary>
        /// Called when service is resumed from pause state.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
            _ftpWatcher.EnableRaisingEvents = true;
        }
        
        /// <summary>
        /// Called when service is stopped.
        /// </summary>
        protected override void OnStop()
        {
            _ftpWatcher.EnableRaisingEvents = false;
            _createdItems.Clear();
            base.OnStop();
        }

        /// <summary>
        /// Called when FileSystemWatcher raises an event(any changes to a file or directory). 
        /// 
        /// Depending on validation results agianst <c>AppSettings["schema"]</c>, a new file is added to either <c>AppSettings["ValidList"]</c> or <c>AppSettings["InvalidList"]</c>
        /// After processing, file is moved into "archive" subdirectory
        /// </summary>
        /// <param name="sender">Object that raised the event that triggered the function call</param>
        /// <param name="e">Event arguments</param>
        private void _ftpWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Changed)   //if file is created or changed
            {
                _createdItems.Add(e.FullPath);
                string SPList = Validate(e.FullPath, schema) ? "ValidList" : "InvalidList";             //decide which list
                addToSpList(e.FullPath, ConfigurationManager.AppSettings[SPList]);                      //add to sharepoint list
                string fileName = (System.IO.File.Exists(archive + e.Name)) ? e.Name + "-new" : e.Name; //change name if duplicate name
                System.IO.File.Move(e.FullPath, archive + fileName);                                    //move to archive
            }
        }
        
        /// <summary>
        /// Validates a given XML agianst a given XSD
        /// </summary>
        /// <param name="xmlUri">XML to be validated</param>
        /// <param name="xsdUri">XSD used for validation</param>
        /// <returns>
        /// Returns true if valid, false if invalid 
        /// </returns>
        private static bool Validate(string xmlUri, string xsdUri)
        {
            try
            {
                //load schema
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("urn:bookstore-schema", xsdUri);
                //load document
                XDocument doc = XDocument.Load(xmlUri);
                string msg = "";
                doc.Validate(schemas, (o, e) =>
                {
                    msg = e.Message;    //record any errors
                });
                if (msg == "")          //empty msg means it passed validation
                {   
                    return true;
                }
                else
                {//if xml fails validation:
                    return false;
                }
            }
            catch (Exception ex)
            {
                //log ex
                return false;
            }
        }
        
        /// <summary>
        /// Adds a file to a SharePoint list. Uses SharePoint server and login detailed in <c>AppSettings</c>
        /// </summary>
        /// <param name="fileName">File to be added</param>
        /// <param name="SPList">Sharepoint list to be added to. This list must be preexisting</param>
        public static void addToSpList(string fileName, string SPList)
        {
            string SPServer = ConfigurationManager.AppSettings["SPServer"];
            string USER = ConfigurationManager.AppSettings["user"];
            string PSWD = ConfigurationManager.AppSettings["password"];
            string DOMAIN = ConfigurationManager.AppSettings["domain"];

            //Create server context
            ClientContext context = new ClientContext(SPServer);

            //Authenticate sharepoint site
            NetworkCredential credentials = new NetworkCredential(USER, PSWD, DOMAIN);
            context.Credentials = credentials;

            Web web = context.Web;

            //Create file to add
            FileCreationInformation newFile = new FileCreationInformation();
            newFile.Content = System.IO.File.ReadAllBytes(fileName);
            newFile.Url = Path.GetFileName(fileName);
            

            //Get destination SP document list 
            List list = web.Lists.GetByTitle(SPList);

            //Add file to SP
            Microsoft.SharePoint.Client.File uploadFile = list.RootFolder.Files.Add(newFile);

            //Load file
            context.Load(uploadFile);

            //Execute context into SP
            context.ExecuteQuery();
        }
    }
}
