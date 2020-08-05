using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Genesys.Azure.WebJobs.Extensions;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;

namespace SampleFunctionApp
{
    public static class GetGenesysOrg
    {
        [FunctionName("GetGenesysOrgByStringToken")]
        public static async Task<IActionResult> RunString(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "org-stringtoken")] HttpRequest req
            , [Genesys] string token
            , ILogger log)
        {
            var org = await GetOrgAsync(token, "mypurecloud.ie");
            return new OkObjectResult(org);
        }

        [FunctionName("GetGenesysOrg")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "org")] HttpRequest req
            , [Genesys] IGenesysAccessToken token
            , ILogger log)
        {
            Organization org = await GetOrgAsync(token.Value, token.Environment);
            return new OkObjectResult(org);
        }

        private static async Task<Organization> GetOrgAsync(string token, string environment)
        {
            var apiClient = new ApiClient($"https://api.{environment}");
            var configuration = new Configuration(apiClient);
            configuration.AccessToken = token;
            var orgApi = new OrganizationApi(configuration);
            var org = await orgApi.GetOrganizationsMeAsync();
            return org;
        }
    }
}
