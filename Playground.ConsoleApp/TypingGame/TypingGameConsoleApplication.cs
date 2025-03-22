using Playground.ConsoleApp.TypingGame.Data.Database;
using Playground.Data.Abstraction;
using Playground.Data.Implementation;
using Playground.Data.LogWriters;
using Playground.Logging;
using Playground.Logging.Abstraction;
using Playground.Logging.Implementation;

namespace Playground.ConsoleApp.TypingGame
{
    internal class TypingGameConsoleApplication : ConsoleApplication
    {
        private static readonly string DirectoryPath = SqliteDatabaseBuilder.DefaultRootDirectory + "Playground/";

        private static readonly string DatabaseName = "TypingGame";

        public TypingGameConsoleApplication(IDataAccess dataAccess, ILoggerFactory loggerFactory) : base(dataAccess, loggerFactory) { }

        public static TypingGameConsoleApplication Create()
        {
            Console.CursorVisible = false;
            Console.Clear();

            var logWriter = new SqliteLogWriter(DirectoryPath, DatabaseName);

            logWriter.CreateTableIfNotExists();

            var loggingFactory = new LoggerFactory(LogLevel.Trace, logWriter);

            var databaseBuilder = new TypingGameDatabaseBuilder(DirectoryPath, DatabaseName, loggingFactory);

            databaseBuilder.BuildDatabase();

            return new TypingGameConsoleApplication(databaseBuilder.NewDataAccess(), loggingFactory);
        }

        protected override void Teardown()
        {

        }
    }
}
