namespace TransactionTrackerAPI.models
{
    public class Notification
    {

        public int IdNotification { get; set; }
        public int RequestId { get; set; }
        public string Document { get; set; }
        public string PayerName { get; set; }
        public string PayerSurname { get; set; }
        public string PayerEmail { get; set; }
        public string PayerMobile { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMessage { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentBank { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
