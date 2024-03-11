namespace CommunityCenterApi.Log
{
    public interface IFileLoggerFactory
    {
        IFileLogger CreateLogger();
    }
    public class FileLoggerFactory : IFileLoggerFactory
    {
        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "logfile.txt");
        public FileLoggerFactory()
        {
            
        }

        public IFileLogger CreateLogger()
        {
            return new FileLogger(path);
        }

    }

}
