using System.Diagnostics;

namespace Playground.Logging.Implementation
{
    [DebuggerDisplay("{Timestamp} | {LogLevel} | {CallingClass} | {CallingMethod} | {Message} | {Extra} ")]
    public class Log
    {
        public int Id { get; set; }

        public LogLevel LogLevel { get; set; }

        public string CallingClass { get; set; } = string.Empty;

        public string CallingMethod { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string? Extra { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}
