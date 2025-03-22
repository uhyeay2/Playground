using System.Runtime.CompilerServices;

namespace Playground.Logging.Abstraction
{
    public interface ILogger<T>
    {
        void LogTrace(string message, object? extra = null, [CallerMemberName] string callingMethod = "");

        void LogDebug(string message, object? extra = null, [CallerMemberName] string callingMethod = "");

        void LogInfo(string message, object? extra = null, [CallerMemberName] string callingMethod = "");

        void LogWarning(string message, object? extra = null, [CallerMemberName] string callingMethod = "");

        void LogError(string message, object? extra = null, [CallerMemberName] string callingMethod = "");

        void LogCritical(string message, object? extra = null, [CallerMemberName] string callingMethod = "");
    }
}
