namespace TransactionTrackerAPI.Resources
{
    public class LogToFile
    {
        public void Log(string message)
        {
            string resourcesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

            if (!Directory.Exists(resourcesDirectory))
            {
                Directory.CreateDirectory(resourcesDirectory);
            }

            string logFilePath = Path.Combine(resourcesDirectory, "logfile.txt");

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now} {message}");
            }
        }
    }
}
