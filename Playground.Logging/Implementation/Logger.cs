using Playground.Logging.Abstraction;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Playground.Logging.Implementation
{
    public class Logger<T> : ILogger<T>, IDisposable where T : class
    {
        private readonly string _className;

        private readonly LogLevel _logLevel;

        private readonly ILogWriter[] _logWriters;

        private readonly ConcurrentQueue<LogEntry> _logQueue = new();
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private bool _disposed = false;

        public Logger(T classToLogFor, LogLevel logLevel, ILogWriter[] logWriters)
        {
            _className = classToLogFor.GetType().Name;
            _logLevel = logLevel;
            _logWriters = logWriters;

            // Logging will be completed on a separate thread.
            Task.Run(ProcessLogQueueAsync);
        }
        ~Logger()
        {
            Dispose();
        }

        public void LogCritical(string message, object? extra = null, [CallerMemberName] string callingMethod = "") => Log(LogLevel.Critical, message, extra, callingMethod);

        public void LogDebug(string message, object? extra = null, [CallerMemberName] string callingMethod = "") => Log(LogLevel.Debug, message, extra, callingMethod);

        public void LogError(string message, object? extra = null, [CallerMemberName] string callingMethod = "") => Log(LogLevel.Error, message, extra, callingMethod);

        public void LogInfo(string message, object? extra = null, [CallerMemberName] string callingMethod = "") => Log(LogLevel.Info, message, extra, callingMethod);

        public void LogTrace(string message, object? extra = null, [CallerMemberName] string callingMethod = "") => Log(LogLevel.Trace, message, extra, callingMethod);

        public void LogWarning(string message, object? extra = null, [CallerMemberName] string callingMethod = "") => Log(LogLevel.Warning, message, extra, callingMethod);

        private void Log(LogLevel logLevel, string message, object? extra, string callingMethod)
        {
            if (logLevel >= _logLevel)
            {
                _logQueue.Enqueue(new LogEntry(logLevel, _className, callingMethod, message, extra, DateTime.UtcNow));
            }
        }

        private async Task ProcessLogQueueAsync()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (_logQueue.TryDequeue(out var log))
                {
                    await SendLogEntryToLogWriters(log);
                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }

        private async Task SendLogEntryToLogWriters(LogEntry log)
        {
            var extraJson = SerializeExtra(log.Extra);

            foreach (var writer in _logWriters)
            {
                try
                {
                    await writer.WriteAsync(log.LogLevel, log.ClassName, log.CallingMethod, log.Message, extraJson, log.Timestamp);
                }
                catch (Exception)
                {
                    //TODO: Handle logging failures?
                }
            }
        }

        private string? SerializeExtra(object? extra)
        {
            if (extra is null) return null;
            if (extra is Exception e) return $"{{ \"Exception\" : \"{e.GetType().Name}\", \"Message\" : \"{e.Message.Replace("\"", "\\\"")}\", \"StackTrace\" : {e.StackTrace} }}";
            try { return JsonSerializer.Serialize(extra); }
            catch (Exception ex) { return $"{{ \"Info\" : \"Error Serializing Extra Details of type {extra.GetType().Name}\", \"Exception\" : \"{ex.GetType().Name}\", \"Message\" : \"{ex.Message.Replace("\"", "\\\"")}\", \"StackTrace\" : {ex.StackTrace} }}"; }
        }

        private record LogEntry(LogLevel LogLevel, string ClassName, string CallingMethod, string Message, object? Extra, DateTime Timestamp);

        public void Dispose()
        {
            if (!_disposed)
            {
                var stopwatch = Stopwatch.StartNew();

                var timeout = TimeSpan.FromSeconds(10);

                while (!_logQueue.IsEmpty && stopwatch.Elapsed < timeout)
                {
                    Thread.Sleep(10);
                }

                _cancellationTokenSource.Cancel();

                if (!_logQueue.IsEmpty)
                {
                    SendLogEntryToLogWriters(new LogEntry(LogLevel.Critical, _className, "Dispose", $"Logger Disposed Before Logging Completed! Logs Lost: {_logQueue.Count}", null, DateTime.UtcNow))
                        .Wait();
                }

                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
