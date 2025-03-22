using Playground.Logging.Abstraction;
using Playground.Data.Abstraction;
using System.Diagnostics;
using Playground.ConsoleUI;

namespace Playground.ConsoleApp
{
    public abstract class ConsoleApplication
    {
        private ConsoleScreen? _currentScreen;

        private readonly IDataAccess _dataAccess;

        private readonly ILoggerFactory _loggerFactory;

        private readonly ILogger<ConsoleApplication> _logger;

        private readonly Stopwatch _stopwatch = new();

        private readonly UserInterface _userInterface;

        protected ConsoleApplication(IDataAccess dataAccess, ILoggerFactory loggerFactory)
        {
            _dataAccess = dataAccess;

            _loggerFactory = loggerFactory;
            
            _logger = _loggerFactory.NewLogger(this);

            _userInterface = new();
        }

        protected virtual void Startup() { }

        protected virtual void Teardown() { }

        public ConsoleScreen? CurrentScreen => _currentScreen;

        public TimeSpan ElapsedTime => _stopwatch.Elapsed;

        public IDataAccess DataAccess => _dataAccess;

        public ILoggerFactory LoggerFactory => _loggerFactory;

        public UserInterface UserInterface => _userInterface;

        public void Run<TEndingScreen>(ConsoleScreen startingScreen) where TEndingScreen : ConsoleScreen
        {
            _logger.LogInfo("Starting Application");

            _stopwatch.Restart();

            _currentScreen = startingScreen;

            try
            {
                _logger.LogTrace("Application Startup Beginning.");

                Startup();

                _logger.LogTrace("Application Startup Completed.");

                while (_currentScreen is not null && _currentScreen.GetType() != typeof(TEndingScreen))
                {
                    _logger.LogInfo("Presenting Screen", _currentScreen.GetType().Name);

                    _currentScreen = _currentScreen.ShowAndGetNext();
                }

                if (_currentScreen is null)
                {
                    _logger.LogWarning("Screen loop ended due to receiving null screen instead of expected ending screen.");
                }
                else
                {
                    _logger.LogInfo("Presenting Screen", _currentScreen.GetType().Name);

                    _currentScreen?.ShowAndGetNext();

                    _logger.LogInfo("Finished Presenting Ending Screen.");
                }

                _logger.LogTrace("Application Teardown Beginning.");

                Teardown();

                _logger.LogTrace("Application Teardown Completed.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("Unhandled Exception", e);

                throw;
            }

            _logger.LogInfo("Shutting Down Application");
        }
    }
}
