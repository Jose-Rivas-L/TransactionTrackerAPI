namespace TransactionTrackerAPI.models
{
    public class Payer
    {
        public Payer(string document, string documentType, string name, string surname, string email, string mobile)
        {
            Document = document;
            DocumentType = documentType;
            Name = name;
            Surname = surname;
            Email = email;
            Mobile = mobile;
        }

        public string Document { get; }
        public string DocumentType { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string Mobile { get; }
    }
}
