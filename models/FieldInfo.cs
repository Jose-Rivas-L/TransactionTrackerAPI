namespace TransactionTrackerAPI.models
{
    public class FieldInfo
    {
        public FieldInfo(string keyword, string value, string displayOn)
        {
            Keyword = keyword;
            Value = value;
            DisplayOn = displayOn;
        }

        public string Keyword { get; }
        public string Value { get; }
        public string DisplayOn { get; }
    }
}
