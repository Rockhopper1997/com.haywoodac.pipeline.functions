using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace com.haywoodac.pipeline.functions
{
    public static class SayHello
    {
        [FunctionName("SayHelloAnon")]
        public static async Task<IActionResult> SayHelloAnon(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SayHelloAnon HTTP trigger called.");

            return new OkObjectResult("Hello anonymous caller");
        }

        [FunctionName("SayHelloPersonalizedAnon")]
        public static async Task<IActionResult> SayHelloPersonalizedAnon(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SayHelloPersonalizedAnon HTTP trigger called.");

            string name = req.Query["name"];

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello {name}";

            return new OkObjectResult(responseMessage);
        }
    }
}
