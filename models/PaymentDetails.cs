namespace TransactionTrackerAPI.models
{
    public class PaymentDetails
    {
        public PaymentDetails(string reference, string description, AmountInfo amount, bool allowPartial, bool subscribe)
        {
            Reference = reference;
            Description = description;
            Amount = amount;
            AllowPartial = allowPartial;
            Subscribe = subscribe;
        }

        public string Reference { get; }
        public string Description { get; }
        public AmountInfo Amount { get; }
        public bool AllowPartial { get; }
        public bool Subscribe { get; }
    }
}
