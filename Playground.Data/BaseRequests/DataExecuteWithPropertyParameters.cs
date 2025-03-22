using Playground.Data.Abstraction;

namespace Playground.Data.BaseRequests
{
    public abstract class DataExecuteWithPropertyParameters : IDataExecute
    {
        public object? GetParameters() => this;

        public abstract string GetSql();
    }
}
