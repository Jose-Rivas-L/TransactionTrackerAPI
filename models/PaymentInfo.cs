namespace TransactionTrackerAPI.models
{
    public class PaymentInfo
    {
        public PaymentInfo(AmountInfo amount, StatusInfo status, string receipt, bool refunded, string franchise, 
            string reference, string issuerName, string authorization, string paymentMethod, List<FieldInfo> processorFields, 
            long internalReference, string paymentMethodName)
        {
            Amount = amount;
            Status = status;
            Receipt = receipt;
            Refunded = refunded;
            Franchise = franchise;
            Reference = reference;
            IssuerName = issuerName;
            Authorization = authorization;
            PaymentMethod = paymentMethod;
            ProcessorFields = processorFields;
            InternalReference = internalReference;
            PaymentMethodName = paymentMethodName;
        }

        public AmountInfo Amount { get; set; }
        public StatusInfo Status { get; set; }
        public string Receipt { get; set; }
        public bool Refunded { get; set; }
        public string Franchise { get; set; }
        public string Reference { get; set; }
        public string IssuerName { get; set; }
        public string Authorization { get; set; }
        public string PaymentMethod { get; set; }
        public List<FieldInfo> ProcessorFields { get; set; }
        public long InternalReference { get; set; }
        public string PaymentMethodName { get; set; }
    }
}
