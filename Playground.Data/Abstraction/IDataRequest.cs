namespace Playground.Data.Abstraction
{
    public interface IDataRequest<T>
    {
        public string GetSql();

        public object? GetParameters();
    }
}
