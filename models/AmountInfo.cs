namespace TransactionTrackerAPI.models
{
    public class AmountInfo
    {
        public string? Currency { get; set; }
        public int? Total { get; set; }

        // Nuevas propiedades opcionales para manejar diferentes formatos
        public AmountDetails? To { get; set; }
        public AmountDetails? From { get; set; }
        public int? Factor { get; set; }
    }

    public class AmountDetails
    {
        public int Total { get; set; }
        public string Currency { get; set; }
    }
}
