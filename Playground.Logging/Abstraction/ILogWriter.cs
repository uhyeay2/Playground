namespace Playground.Logging.Abstraction
{
    public interface ILogWriter
    {
        public Task WriteAsync(LogLevel logLevel, string callingClass, string callingMethod, string message, string? extra, DateTime timestamp);
    }
}
