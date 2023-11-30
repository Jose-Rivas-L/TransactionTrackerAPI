namespace TransactionTrackerAPI.models
{
    public class PaymentNotification
    {
        public PaymentNotification(int requestId, StatusInfo status, RequestInfo request, List<PaymentInfo> payments, string subscription)
        {
            RequestId = requestId;
            Status = status;
            Request = request;
            Payments = payments;
            Subscription = subscription;
        }

        public int RequestId { get; }
        public StatusInfo Status { get; }
        public RequestInfo Request { get; }
        public List<PaymentInfo> Payments { get; }
        public string Subscription { get; }
    }
}
