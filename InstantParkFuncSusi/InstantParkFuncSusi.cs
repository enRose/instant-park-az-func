using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;

namespace InstantParkFuncSusi
{
    public static class InstantParkFuncSusi
    {
        private static string ConnectionString =>
            Environment.GetEnvironmentVariable("sqldb_connection",
                EnvironmentVariableTarget.Process);


        [FunctionName("InstantParkFuncSusi")]
        public static async Task<IActionResult> Run(
            [
                HttpTrigger(
                AuthorizationLevel.Function, "get", "post",
                Route = null
            )] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    return (ActionResult)new OkObjectResult($"The database connection is, " +
                        $"{connection.State}");
                }
            }
            catch (SqlException sqlex)
            {
                return new BadRequestObjectResult(
                   $"The following SqlException happened:{sqlex.Message}");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(
                   $"The following SqlException happened:{ex.Message}");
            }            
        }
    }
}
