using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace fnPostDatabase
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("SaveMovie")]
        //[CosmosDBOutput("%DatabaseName%", "movies", Connection = "CosmosDbConnection", CreateIfNotExists = true, PartitionKey = "title")]
        [CosmosDBOutput("%DatabaseName%", "%ContainerName%", Connection = "CosmosDbConnection", CreateIfNotExists = true, PartitionKey = "title")]
        public async Task<object?> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            MovieRequest movie = new MovieRequest();
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                movie = JsonConvert.DeserializeObject<MovieRequest>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu a seguinte exceção: {ex.Message}");
            }

            return JsonConvert.SerializeObject(movie);
        }
    }
}
