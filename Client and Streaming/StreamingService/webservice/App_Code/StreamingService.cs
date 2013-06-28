using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Web.Hosting;

[ServiceBehavior]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
/// class for the wcf service
public class StreamingService : IStreamingService
{
    /// <summary>
    /// Funtion for client to call for uploading xml file.
    /// takes a file stream as input and ouputs a 
    /// string response in xml format with a message 
    /// element, errors element and succesful element 
    /// on the status of the upload.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public string UploadStream(System.IO.Stream stream)
    {
        // create vars for validation
        bool bIsValid;
        XmlNode currentRow;
        XmlDocument xmlDoc = new XmlDocument();
        string strErrors = "";

        //create return xmlDoc
        XmlElement parentNode = xmlDoc.CreateElement("upload");;
        CreateReturnXml(xmlDoc, parentNode);

        //this implementation places the uploaded xml file
        //in the bin directory and gives it a unique id filename
        //To Do:should add a meaningful string to filename
        string filePath = HostingEnvironment.ApplicationPhysicalPath + ConfigurationManager.AppSettings["bin"].ToString() + Guid.NewGuid() + ".xml";

        // XmlWriter is not in using block so as to
        // differentiate between xsd validation problem
        // and xml is not well formed problem
        XmlWriter writer = XmlWriter.Create(filePath);
        try
        {
            // creating xmlreader and xmlreaderSetting to validate 
            // against and xsd file 
            String xsdUri = HostingEnvironment.ApplicationPhysicalPath + ConfigurationManager.AppSettings["XSDPath"].ToString() + "books.xsd";
            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.ValidationType = ValidationType.Schema;
            xmlSettings.Schemas.Add(null, xsdUri);
            // xml validation handler
            xmlSettings.ValidationEventHandler += delegate(object sender, ValidationEventArgs vargs)
            {
                strErrors += vargs.Message + "\n";
                bIsValid = false;
            };

            // An using block to close xmlreader and xmlwritter 
            // in case of an exception
            using (XmlReader reader = XmlReader.Create(stream, xmlSettings))
            {
                //create xmlDocument used to read a node from reader
                XmlDocument doc = new XmlDocument();

                // creating  xmlWriter setting
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Indent = true;
                writer.Close();
                writer = XmlWriter.Create(filePath, xws);

                // ToFix: find a way to read, validate, and
                // write xml at same time with out using 
                // xmlNode (right now loading one xmlNode 
                // at a time)

                //loop in the reader until end of xmlreader is reached
                while (reader.Read())
                {
                    // set default of the read to successful
                    bIsValid = true;
                    // read a xmlnode 
                    currentRow = doc.ReadNode(reader);
                    // if it is a valid node write to xmlwriter
                    if (bIsValid)
                    {
                        currentRow.WriteTo(writer);
                    }
                    // close xml reader, writer, and the stream 
                    // and delete the file and return
                    else
                    {
                        writer.Close();
                        stream.Close();
                        System.IO.File.Delete(filePath);
                        // retrieve the text for return
                        return XsdReturnMessage(xmlDoc, strErrors, parentNode);
                    }
                }
                // xml processed successfully 
                // close xml reader, writer, and stream
                writer.Close();
                stream.Close();
                return ValidXml(xmlDoc, filePath, parentNode);
            }
        }
        // if process not sucessful
        // Delete file
        catch (Exception ex)
        {
            ((IDisposable)writer).Dispose();
            stream.Close();
            System.IO.File.Delete(filePath);
            // retrieve the text for return
            return XmlNotWellFormed(xmlDoc, strErrors, parentNode);
        }
        finally
        {
            ((IDisposable)writer).Dispose();
            stream.Close();
        }
    }
    /// <summary>
    /// Change the xmlDoc to a string for the client
    /// </summary>
    /// <param name="myxml"></param>
    /// <returns></returns>
    private string GetXMLAsString(XmlDocument myxml)
    {
        StringWriter sw = new StringWriter();
        XmlTextWriter tx = new XmlTextWriter(sw);
        myxml.WriteTo(tx);

        string str = sw.ToString();// 
        return str;
    }
    /// <summary>
    /// uploads validated files to sharepoint libary
    /// To Fix: upload of 5mb fails and returns a 
    /// Error: (400) Bad Request
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="SPList"></param>
    private void AddToSP(string fileName, string SPList)
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
    /// <summary>
    /// setup the xmlDocument that holds the data to be return to the 
    /// client
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="parentNode"></param>
    private void CreateReturnXml(XmlDocument xmlDoc, XmlElement parentNode)
    {
        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
        // Create the root element
        XmlElement rootNode = xmlDoc.CreateElement("UploadReport");
        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
        xmlDoc.AppendChild(rootNode);
        // Create a new <Errors> element and add it to the root node
        xmlDoc.DocumentElement.PrependChild(parentNode);
    }
    /// <summary>
    /// creates the xmlDocument with return data: messagge,
    /// errors, successful for xml that fail xsd validation
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="strErrors"></param>
    /// <param name="parentNode"></param>
    /// <returns></returns>
    private string XsdReturnMessage(XmlDocument xmlDoc, string strErrors, XmlElement parentNode)
    {
        // Create the required nodes for return
        XmlElement messageNode = xmlDoc.CreateElement("Message");
        XmlElement errorsNode = xmlDoc.CreateElement("Errors");
        XmlElement successfulNode = xmlDoc.CreateElement("Successful");
        XmlText meassageText;
        XmlText errorsText;
        XmlText successfulText;
        // retrieve the text for return
        meassageText = xmlDoc.CreateTextNode("The xml does not conform to xsd");
        errorsText = xmlDoc.CreateTextNode(strErrors);
        successfulText = xmlDoc.CreateTextNode("false");
        // append the nodes to the parentNode without the value
        parentNode.AppendChild(messageNode);
        parentNode.AppendChild(errorsNode);
        parentNode.AppendChild(successfulNode);
        // save the value of the fields into the nodes
        messageNode.AppendChild(meassageText);
        errorsNode.AppendChild(errorsText);
        successfulNode.AppendChild(successfulText);
        return GetXMLAsString(xmlDoc);
    }
    /// <summary>
    /// creates the xmlDocument with return data: messagge,
    /// errors, successful for xml that pass validation
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="filePath"></param>
    /// <param name="parentNode"></param>
    /// <returns></returns>
    private string ValidXml(XmlDocument xmlDoc, string filePath, XmlElement parentNode)
    {
        // Create the required nodes for return
        XmlElement messageNode = xmlDoc.CreateElement("Message");
        XmlElement errorsNode = xmlDoc.CreateElement("Errors");
        XmlElement successfulNode = xmlDoc.CreateElement("Successful");
        XmlText meassageText;
        XmlText errorsText;
        XmlText successfulText;
        // copy file in to sharepoint libary
        try
        {
            AddToSP(filePath, ConfigurationManager.AppSettings["SPList"]);
            meassageText = xmlDoc.CreateTextNode("File upload and validated");
            errorsText = xmlDoc.CreateTextNode("No Errors");
            successfulText = xmlDoc.CreateTextNode("true");
        }
        catch (Exception ex)
        {
            meassageText = xmlDoc.CreateTextNode("file failed upload to sharepoint.");
            errorsText = xmlDoc.CreateTextNode(ex.ToString());
            successfulText = xmlDoc.CreateTextNode("false");
        }
        // append the nodes to the parentNode without the value
        parentNode.AppendChild(messageNode);
        parentNode.AppendChild(errorsNode);
        parentNode.AppendChild(successfulNode);
        // save the value of the fields into the nodes
        messageNode.AppendChild(meassageText);
        errorsNode.AppendChild(errorsText);
        successfulNode.AppendChild(successfulText);
        // delete the file from local dir
        System.IO.File.Delete(filePath);
        return GetXMLAsString(xmlDoc);
    }
    /// <summary>
    /// creates the xmlDocument with return data: messagge,
    /// errors, successful for xml that are not well formed
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="strErrors"></param>
    /// <param name="parentNode"></param>
    /// <returns></returns>
    private string XmlNotWellFormed(XmlDocument xmlDoc, string strErrors, XmlElement parentNode)
    {
        // Create the required nodes for return
        XmlElement messageNode = xmlDoc.CreateElement("Message");
        XmlElement errorsNode = xmlDoc.CreateElement("Errors");
        XmlElement successfulNode = xmlDoc.CreateElement("Successful");
        XmlText meassageText;
        XmlText errorsText;
        XmlText successfulText;
        meassageText = xmlDoc.CreateTextNode("An exception was thrown while trying to upload. \nPlease check that the xml is well formed.");
        errorsText = xmlDoc.CreateTextNode(strErrors);
        successfulText = xmlDoc.CreateTextNode("false");
        // append the nodes to the parentNode without the value
        parentNode.AppendChild(messageNode);
        parentNode.AppendChild(errorsNode);
        parentNode.AppendChild(successfulNode);
        // save the value of the fields into the nodes
        messageNode.AppendChild(meassageText);
        errorsNode.AppendChild(errorsText);
        successfulNode.AppendChild(successfulText);
        return GetXMLAsString(xmlDoc);
    }
}
