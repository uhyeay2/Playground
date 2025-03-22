using Playground.Data.Abstraction;
using System.Data;
using System.Data.SQLite;

namespace Playground.Data.Implementation
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string _dataSource;

        public SqliteConnectionFactory(string dataSource) => _dataSource = dataSource;

        public IDbConnection CreateConnection() => new SQLiteConnection("dataSource=" + _dataSource);
    }
}
