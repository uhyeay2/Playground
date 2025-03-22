using Playground.Data.Abstraction;
using Playground.Logging.Abstraction;
using System.Data.SQLite;

namespace Playground.Data.Implementation
{
    public abstract class SqliteDatabaseBuilder
    {
        public static readonly string DefaultRootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/";

        private readonly string _directoryPath;

        private readonly string _databaseName;

        private readonly ILoggerFactory _loggerFactory;

        protected readonly ILogger<SqliteDatabaseBuilder> _logger;

        public SqliteDatabaseBuilder(string directoryPath, string databaseName, ILoggerFactory loggerFactory)
        {
            _directoryPath = directoryPath;

            _databaseName = databaseName;
            
            _loggerFactory = loggerFactory;

            _logger = _loggerFactory.NewLogger(this);
        }

        public string DirectoryPath => _directoryPath;

        public string DatabaseName => _databaseName;

        public void BuildDatabase()
        {
            _logger.LogTrace("Beginning Database Creation.");

            Directory.CreateDirectory(DirectoryPath);

            if (!File.Exists(DirectoryPath + DatabaseName))
            {
                _logger.LogInfo("Database file does not exist. Creating new file.", new { DirectoryPath, DatabaseName });

                SQLiteConnection.CreateFile(DirectoryPath + DatabaseName);

                _logger.LogInfo("Database file created.");
            }
            else
            {
                _logger.LogInfo("Database found.", new { DirectoryPath, DatabaseName });
            }

            _logger.LogTrace("Starting Database Table Creation.");

            CreateTablesIfNotExists();

            _logger.LogTrace("Completed Database Table Creation.");
        }

        /// <summary>
        /// Deletes the database file.
        /// </summary>
        public void PurgeDatabase()
        {
            if (File.Exists(DirectoryPath + DatabaseName))
            {
                File.Delete(DirectoryPath + DatabaseName);
            }
        }

        public IDbConnectionFactory NewConnectionFactory() => new SqliteConnectionFactory(DirectoryPath + DatabaseName);

        public IDataAccess NewDataAccess() => new DataAccess(NewConnectionFactory(), _loggerFactory);

        public abstract void CreateTablesIfNotExists();

    }
}
