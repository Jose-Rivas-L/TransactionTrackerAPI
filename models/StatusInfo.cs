namespace TransactionTrackerAPI.models
{
    public class StatusInfo
    {
        public StatusInfo(string status, string reason, string message, DateTime date)
        {
            Status = status;
            Reason = reason;
            Message = message;
            Date = date;
        }

        public string Status { get; }
        public string Reason { get;  }
        public string Message { get; }
        public DateTime Date { get; }
    }
}
