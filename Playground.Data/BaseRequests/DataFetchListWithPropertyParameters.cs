using Playground.Data.Abstraction;

namespace Playground.Data.BaseRequests
{
    public abstract class DataFetchListWithPropertyParameters<T> : IDataFetchList<T>
    {
        public object? GetParameters() => this;

        public abstract string GetSql();
    }
}
