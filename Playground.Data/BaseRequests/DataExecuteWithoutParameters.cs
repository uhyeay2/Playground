using Playground.Data.Abstraction;

namespace Playground.Data.BaseRequests
{
    public abstract class DataExecuteWithoutParameters : IDataExecute
    {
        public object? GetParameters() => null;

        public abstract string GetSql();
    }
}
