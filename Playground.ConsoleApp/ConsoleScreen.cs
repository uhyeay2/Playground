using Playground.ConsoleUI.Core;
using Playground.Logging.Abstraction;

namespace Playground.ConsoleApp
{
    public abstract class ConsoleScreen
    {
        protected readonly ConsoleApplication _app;

        protected readonly ILogger<ConsoleScreen> _logger;

        protected ConsoleScreen(ConsoleApplication consoleApplication)
        {
            _app = consoleApplication;

            _logger = consoleApplication.LoggerFactory.NewLogger(this);
        }

        protected abstract ConsoleScreen GetNextScreen();

        protected virtual void Startup() { }

        protected virtual void Update() { }

        protected virtual void Shutdown() { }

        protected virtual bool IsFinished() => true;

        protected void Display(params IDisplayComponent[] components) => _app.UserInterface.Display(components);

        public ConsoleScreen ShowAndGetNext()
        {
            _logger.LogTrace("Showing ConsoleScreen");

            Startup();

            _logger.LogTrace("ConsoleScreen Startup Completed.");

            while (!IsFinished())
            {
                _logger.LogTrace("ConsoleScreen is not finished. Executing Update() method.");

                Update();

                _logger.LogTrace("ConsoleScreen Update Completed.");
            }

            _logger.LogTrace("ConsoleScreen is finished");

            Shutdown();

            _logger.LogTrace("ConsoleScreen Shutdown Completed");

            var nextScreen = GetNextScreen();

            _logger.LogTrace($"Next Screen: {nextScreen.GetType().Name}");

            return nextScreen;
        }
    }
}
