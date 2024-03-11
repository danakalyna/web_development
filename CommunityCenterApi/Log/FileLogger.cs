namespace CommunityCenterApi.Log
{
    public interface IFileLogger : IDisposable
    {
        void WriteTextToFile(string message);
    }
    public class FileLogger : IFileLogger
    {
        private readonly string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public void Dispose()
        {

        }

        public void WriteTextToFile(string message)
        {
            lock (filePath)
            {
                File.AppendAllText(filePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
        }
    }

}
