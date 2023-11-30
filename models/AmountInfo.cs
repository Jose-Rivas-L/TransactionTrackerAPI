namespace TransactionTrackerAPI.models
{
    public class AmountInfo
    {
        public AmountInfo(string currency, int total)
        {
            Currency = currency;
            Total = total;
        }

        public string Currency { get;}
        public int Total { get; }
    }
}
