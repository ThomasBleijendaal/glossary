using System.Threading.Tasks;
using InProcessFunctionApp.ReadFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace InProcessFunctionApp
{
    public static class HttpTrigger
    {
        [FunctionName("HttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [ReadFile("./TestData.json")] string data,
            ILogger log)
        {
            return new OkObjectResult(data);
        }
    }
}
