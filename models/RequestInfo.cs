using System.Reflection;

namespace TransactionTrackerAPI.models
{
    public class RequestInfo
    {
        public RequestInfo(string locale, Payer payer, PaymentDetails payment, List<FieldInfo> fields, string ipAddress, string userAgent, DateTime expiration)
        {
            Locale = locale;
            Payer = payer;
            Payment = payment;
            Fields = fields;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            Expiration = expiration;
        }

        public string Locale { get; }
        public Payer Payer { get;}
        public PaymentDetails Payment { get; }
        public List<FieldInfo> Fields { get; }
        public string IpAddress { get; }
        public string UserAgent { get; }
        public DateTime Expiration { get; }
    }
}
