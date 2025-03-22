using Playground.Data.Abstraction;

namespace Playground.Data.BaseRequests
{
    public abstract class DataFetchWithPropertyParameters<T> : IDataFetch<T>
    {
        public object? GetParameters() => this;

        public abstract string GetSql();
    }
}
