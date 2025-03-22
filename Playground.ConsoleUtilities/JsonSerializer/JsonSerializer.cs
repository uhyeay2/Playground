using Playground.Logging.Abstraction;

namespace Playground.ConsoleUtilities.JsonSerializer
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly ILogger<JsonSerializer> _logger;

        public JsonSerializer(ILoggerFactory loggerFactory) => _logger = loggerFactory.NewLogger(this);

        public T? Deserialize<T>(string value)
        {
            var type = typeof(T);

            _logger.LogTrace($"Deserializing [{type}] From: {value}");

            try
            {
                var obj = System.Text.Json.JsonSerializer.Deserialize<T>(value);

                _logger.LogTrace($"Deserialized [{type}] Successfully.");

                return obj;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Exception Caught When Deserializing {type} from: {value} -- Exception Message: {e.Message} -- Stack Trace: {e.StackTrace}");

                throw;
            }
        }

        public string Serialize(object value)
        {
            if (value == null)
            {
                _logger.LogWarning("Attempted to serialize null object.");

                return string.Empty;
            }

            var type = value.GetType().Name;

            _logger.LogTrace($"Attempting to serialize object: {type}");

            var result = System.Text.Json.JsonSerializer.Serialize(value);

            _logger.LogTrace($"Serialized {type} Successfully. String: {result}");

            return result;
        }
    }
}
