namespace TransactionTrackerAPI.models
{
    public class PaymentNotification
    {
        public PaymentNotification(int requestId, StatusInfo status, RequestInfo request, List<PaymentInfo> payment, string subscription)
        {
            RequestId = requestId;
            Status = status;
            Request = request;
            Payment = payment;
            Subscription = subscription;
        }

        public int RequestId { get; }
        public StatusInfo Status { get; }
        public RequestInfo Request { get; }
        public List<PaymentInfo> Payment { get; }
        public string Subscription { get; }
    }
}
