using Playground.Data.Abstraction;
using Playground.Logging.Abstraction;
using System.Data;
using Dapper;

namespace Playground.Data.Implementation
{
    public class DataAccess : IDataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<DataAccess> _logger;

        public DataAccess(IDbConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            _connectionFactory = connectionFactory;

            _logger = loggerFactory.NewLogger(this);
        }

        public int Execute(IDataExecute request) => 
            HandleRequest(request, _ => _.Execute(request.GetSql(), request.GetParameters()));

        public TResponse? Fetch<TResponse>(IDataFetch<TResponse> request) => 
            HandleRequest(request, _ => _.QueryFirstOrDefault<TResponse>(request.GetSql(), request.GetParameters()));

        public IEnumerable<TResponse> FetchList<TResponse>(IDataFetchList<TResponse> request) => 
            HandleRequest(request, _ => _.Query<TResponse>(request.GetSql(), request.GetParameters())) ?? [];

        public async Task<int> ExecuteAsync(IDataExecute request) => 
            await HandleRequestAsync(request, async _ => await _.ExecuteAsync(request.GetSql(), request.GetParameters()));
        
        public async Task<TResponse?> FetchAsync<TResponse>(IDataFetch<TResponse> request) => 
            await HandleRequestAsync(request, async _ => await _.QueryFirstOrDefaultAsync<TResponse?>(request.GetSql(), request.GetParameters()));

        public async Task<IEnumerable<TResponse>> FetchListAsync<TResponse>(IDataFetchList<TResponse> request) => 
            await HandleRequestAsync(request, async _ => await _.QueryAsync<TResponse>(request.GetSql(), request.GetParameters())) ?? [];

        private TResponse? HandleRequest<TResponse>(IDataRequest<TResponse> request, Func<IDbConnection, TResponse?> func)
        {
            if (request == null)
            {
                _logger.LogError("NULL request sent to DataAccess.");

                throw new ArgumentNullException(nameof(request));
            }

            var requestName = request.GetType().Name;

            try
            {
                _logger.LogTrace("Creating IDBConnection.");

                using var connection = _connectionFactory.CreateConnection();

                _logger.LogTrace("Opening Connection");

                connection.Open();

                _logger.LogInfo($"Sending IDataRequest to database", requestName);

                _logger.LogDebug($"IDataRequest Details", new 
                { 
                    RequestType = requestName, 
                    Sql = request.GetSql(), 
                    Parameters = request.GetParameters()
                });

                var result = func.Invoke(connection);

                var responseType = typeof(TResponse);

                _logger.LogInfo("Received IDataRequest Result", responseType.Name);

                _logger.LogDebug("IDataRequest Result Details", new
                {
                    Type = responseType.Name,
                    Value = result
                });

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Caught When Handling IDataRequest", new 
                { 
                    RequestType = requestName,
                    Sql = request.GetSql(),
                    Parameters = request.GetParameters(),
                    Exception = e.GetType().Name,
                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace
                });

                throw;
            }
        }

        private async Task<TResponse?> HandleRequestAsync<TResponse>(IDataRequest<TResponse> request, Func<IDbConnection, Task<TResponse?>> func)
        {
            if (request == null)
            {
                _logger.LogError("NULL request sent to DataAccess.");

                throw new ArgumentNullException(nameof(request));
            }

            var requestName = request.GetType().Name;

            try
            {
                _logger.LogTrace("Creating IDBConnection.");

                using var connection = _connectionFactory.CreateConnection();

                _logger.LogTrace("Opening Connection");

                connection.Open();

                _logger.LogInfo($"Sending IDataRequest to database", requestName);

                _logger.LogDebug($"IDataRequest Details", new
                {
                    RequestType = requestName,
                    Sql = request.GetSql(),
                    Parameters = request.GetParameters()
                });

                var result = await func.Invoke(connection);

                var responseType = typeof(TResponse);

                _logger.LogInfo("Received IDataRequest Result", responseType);

                _logger.LogDebug("IDataRequest Result Details", new
                {
                    Type = responseType,
                    Value = result
                });

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Caught When Handling IDataRequest", new
                {
                    RequestType = requestName,
                    Sql = request.GetSql(),
                    Parameters = request.GetParameters(),
                    Exception = e.GetType().Name,
                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace
                });

                throw;
            }
        }
    }
}
