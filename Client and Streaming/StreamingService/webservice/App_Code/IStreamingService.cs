using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the interface name "IStreamingService" here, you must also update the reference to "IStreamingService" in Web.config.
[ServiceContract]
public interface IStreamingService
{

    [OperationContract]
    string UploadStream(System.IO.Stream stream);

}
 
