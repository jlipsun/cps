
using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using System.Xml.Serialization;
using StreamingService;

namespace Microsoft.Samples.Stream
{
    //The service contract is defined in generatedClient.cs, generated from the service by the svcutil tool.

    // toFix: add multithreading

    class GoodXml
    {
        // This method that will be called when the thread is started
        public void Beta()
        {
            //String[] requests = Directory.GetFiles(@"C:\xmls and xsd\", "*Exam Request*");
            String[] requests = Directory.GetFiles(@"C:\xmls and xsd\", "*).xml");

            foreach (String request in requests)
            {
                StreamingServiceClient client1 = new StreamingServiceClient();

                Console.WriteLine("------ Using HTTP ------ ");

                Console.WriteLine("Calling UploadStream()");
                FileStream instream1 = File.OpenRead(request);
                string result1 = client1.UploadStream(instream1);

                Console.WriteLine(result1);

                instream1.Close();

                client1.Close();

            }
            //int count = 1;

            //while (count < 26)
            //{
            //    string filePath1 = "C:\\xmls and xsd\\test - Copy (" + count + ").xml";
            //    // Create a client with given client endpoint configuration

            //    StreamingServiceClient client1 = new StreamingServiceClient();

            //    Console.WriteLine("------ Using HTTP ------ ");

            //    Console.WriteLine("Calling UploadStream()");
            //    FileStream instream1 = File.OpenRead(filePath1);
            //    string result1 = client1.UploadStream(instream1);

            //    Console.WriteLine(result1);

            //    instream1.Close();

            //    client1.Close();
            //    count++;
            //}
        }
    }

    class BadData
    {
        // This method that will be called when the thread is started
        public void Beta()
        {
            string filePath2 = "C:\\Users\\Administrator\\Documents\\Visual Studio 2012\\Projects\\5mbBad.xml";
            while (true)
            {
                // Create a client with given client endpoint configuration

                StreamingServiceClient client2 = new StreamingServiceClient();

                Console.WriteLine("------ Using HTTP ------ ");

                Console.WriteLine("Calling UploadStream()");
                FileStream instream2 = File.OpenRead(filePath2);
                string result2 = client2.UploadStream(instream2);

                Console.WriteLine(result2);

                instream2.Close();

                client2.Close();
            }
        }
    }

    class BadXml
    {
        // This method that will be called when the thread is started
        public void Beta()
        {
            string filePath3 = "C:\\Users\\Administrator\\Documents\\Visual Studio 2012\\Projects\\5mbBad2.xml";
            while (true)
            {
                StreamingServiceClient client3= new StreamingServiceClient();

                Console.WriteLine("------ Using Custom HTTP ------ ");

                Console.WriteLine("Calling UploadStream()");
                FileStream instream3 = File.OpenRead(filePath3);
                string result3 = client3.UploadStream(instream3);

                Console.WriteLine(result3);

                instream3.Close();

                client3.Close();
            }
        }
    }

    //Client implementation code.
    class Client
    {
        static void Main() //public void clientThread()
        {
            Console.WriteLine("Press <ENTER> when service is ready");
            Console.ReadLine();

            // create good xml file threading
            GoodXml oAlpha = new GoodXml();

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            Thread oThread1 = new Thread(new ThreadStart(oAlpha.Beta));

            // Start the thread
            oThread1.Start();

            // bad Data that does not conform to xsd thread
            //BadData oBeta = new BadData();

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            //Thread oThread2 = new Thread(new ThreadStart(oBeta.Beta));

            // Start the thread
            //oThread2.Start();

            // bad xml Format thread
            //BadXml oDelta = new BadXml();

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            //Thread oThread3 = new Thread(new ThreadStart(oDelta.Beta));

            // Start the thread
            //oThread3.Start();

            // Spin for a while waiting for the started thread to become
            // alive:
            while (!oThread1.IsAlive) ;
            //while (!oThread2.IsAlive) ;
            //while (!oThread3.IsAlive) ;

            // Put the Main thread to sleep for 1 millisecond to allow oThread
            // to do some work:
            Thread.Sleep(100000);

            // Request that oThread be stopped
            oThread1.Abort();
            //oThread2.Abort();
            //oThread3.Abort();

            // Wait until oThread finishes. Join also has overloads
            // that take a millisecond interval or a TimeSpan object.
            oThread1.Join();
            //oThread2.Join();
            //oThread3.Join();

            Console.WriteLine();
            Console.WriteLine("GoodXml.Beta has finished");
            Console.WriteLine("BadData.Beta has finished");
            Console.WriteLine("BadXml.Beta has finished");

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to terminate client.");
            Console.ReadLine();
        }
        
    }
}

