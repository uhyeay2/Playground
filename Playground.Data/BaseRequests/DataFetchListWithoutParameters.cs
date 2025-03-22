using Playground.Data.Abstraction;

namespace Playground.Data.BaseRequests
{
    public abstract class DataFetchListWithoutParameters<T> : IDataFetchList<T>
    {
        public object? GetParameters() => null;

        public abstract string GetSql();
    }
}
