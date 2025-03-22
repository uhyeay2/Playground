using Playground.Data.Abstraction;

namespace Playground.Data.BaseRequests
{
    public abstract class DataFetchWithoutParameters<T> : IDataFetch<T>
    {
        public object? GetParameters() => null;

        public abstract string GetSql();
    }
}
