using System.ServiceModel;

namespace GpsEmulator.GpsService
{
    [ServiceContract]
    interface IGpsEmulatorService
    {
        [OperationContract]
        string GetCurrentPosition();
    }
}
