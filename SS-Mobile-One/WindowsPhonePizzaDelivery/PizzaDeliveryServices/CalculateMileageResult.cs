using System.Runtime.Serialization;

namespace PizzaDeliveryServices
{
    [DataContract]
    public class CalculateMileageResult
    {
        public CalculateMileageResult()
        {
        }

        public CalculateMileageResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public CalculateMileageResult(bool success, string message, double mileage)
        {
            Success = success;
            Message = message;
            Mileage = mileage;
        }

        [DataMember]
        public bool Success { get; private set; }
        [DataMember]
        public string Message { get; private set; }
        [DataMember]
        public double Mileage { get; set; }
    }
}