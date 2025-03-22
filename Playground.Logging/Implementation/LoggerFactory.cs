using Playground.Logging.Abstraction;

namespace Playground.Logging.Implementation
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly ILogWriter[] _logWriters;

        private readonly LogLevel _logLevel;

        public LoggerFactory(LogLevel logLevel, params ILogWriter[] logWriters)
        {
            _logLevel = logLevel;

            _logWriters = logWriters;
        }

        public ILogger<T> NewLogger<T>(T classToLogFor) where T : class => new Logger<T>(classToLogFor, _logLevel, _logWriters);
        
    }
}
