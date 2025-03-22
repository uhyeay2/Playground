using Dapper;
using Playground.Data.Implementation;
using Playground.Logging;
using Playground.Logging.Abstraction;
using System.Data.SQLite;

namespace Playground.Data.LogWriters
{
    public class SqliteLogWriter : ILogWriter
    {
        private readonly SqliteConnectionFactory _connectionFactory;

        private readonly string _tableName;

        private readonly string _directoryPath;

        private readonly string _databaseName;

        public SqliteLogWriter(string directoryPath, string databaseName, string tableName = "Logs")
        {
            _connectionFactory = new(directoryPath + databaseName);
            _directoryPath = directoryPath;
            _databaseName = databaseName;
            _tableName = tableName;
        }

        public string DirectoryPath => _directoryPath;
        public string DatabaseName => _databaseName;
        public string TableName => _tableName;

        public void CreateTableIfNotExists()
        {
            Directory.CreateDirectory(DirectoryPath);

            if (!File.Exists(DirectoryPath + DatabaseName))
            {
                SQLiteConnection.CreateFile(DirectoryPath + DatabaseName);
            }

            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            connection.Execute(
                @$"
                        CREATE TABLE IF NOT EXISTS {TableName} (
                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                        LogLevel INTEGER NOT NULL,
                        CallingClass TEXT NOT NULL,
                        CallingMethod TEXT NOT NULL,
                        Message TEXT NOT NULL,
                        Extra TEXT NULL,
                        Timestamp TEXT NOT NULL
                    );
                "
            );
        }

        public async Task WriteAsync(LogLevel logLevel, string callingClass, string callingMethod, string message, string? extra, DateTime timestamp)
        {
            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            await connection.ExecuteAsync(
                $@"
                    INSERT INTO {TableName} (  LogLevel,  CallingClass,  CallingMethod,  Message,  Extra,  Timestamp )
                                     VALUES ( @logLevel, @callingClass, @callingMethod, @message, @extra, @timestamp );
                ",
                param: new { logLevel, callingClass, callingMethod, message, extra, timestamp }
            );
        }
    }
}
