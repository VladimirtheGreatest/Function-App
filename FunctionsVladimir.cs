using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppVladimir
{
    public static class FunctionsVladimir
    {
        [FunctionName("BeautifulFunction")]
        [ProducesResponseType(200)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string name = req.Query["name"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;

                string responseMessage = string.IsNullOrEmpty(name)
                    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                    : $"Hello, {name}. This HTTP triggered function executed successfully.";

                return new OkObjectResult(responseMessage);
            }
            catch (Exception)
            {
                log.LogError($"FAILED: for request {req}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            } 
        }
        /// <summary>
        /// this function should remind be to eat every 3 hours to maximize my gains in the gym
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName("TimerTriggerVladimir")]
        public static void Eat([TimerTrigger("0 7-22/3 * * *")] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Eat some food");
            }
            log.LogInformation($"C# Timer trigger bulking function executed at: {DateTime.Now}");
        }
        [FunctionName("DailyStandUpVladimir")]
        public static void StandUp([TimerTrigger("0 30 9 * * 1-5")] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }
            log.LogInformation($"C# Timer trigger daily standup function executed at: {DateTime.Now}");
        }
        [FunctionName("RandomTimerVladimir")]
        public static void Time([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("2 minutes gone");
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
