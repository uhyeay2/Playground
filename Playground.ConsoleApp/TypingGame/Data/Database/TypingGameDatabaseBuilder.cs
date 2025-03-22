using Playground.Data.Implementation;
using Playground.Logging.Abstraction;

namespace Playground.ConsoleApp.TypingGame.Data.Database
{
    internal class TypingGameDatabaseBuilder : SqliteDatabaseBuilder
    {
        public TypingGameDatabaseBuilder(string directoryPath, string databaseName, ILoggerFactory loggerFactory) : base(directoryPath, databaseName, loggerFactory)
        {
        }

        public override void CreateTablesIfNotExists()
        {
            var dataAccess = NewDataAccess();

            //TODO: Create Tables
        }
    }
}
