using System.ServiceModel;

namespace GpsEmulatorClient
{
    [ServiceContract]
    interface IGpsEmulatorService
    {
        [OperationContract]
        string GetCurrentPosition();
    }
}
