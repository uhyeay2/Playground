namespace Playground.Logging.Abstraction
{
    public interface ILoggerFactory
    {
        public ILogger<T> NewLogger<T>(T classToLogFor) where T : class;
    }
}
