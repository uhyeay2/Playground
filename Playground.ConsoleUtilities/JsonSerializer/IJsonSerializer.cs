namespace Playground.ConsoleUtilities.JsonSerializer
{
    public interface IJsonSerializer
    {
        public string Serialize(object value);

        public T? Deserialize<T>(string value);
    }
}
