using System.Data;

namespace Playground.Data.Abstraction
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}
