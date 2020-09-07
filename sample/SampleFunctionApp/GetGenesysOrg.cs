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
using System.Net.Http;

namespace SampleFunctionApp
{
    public class GetGenesysOrg
    { 

        [FunctionName("GetGenesysOrgByStringToken")]
        public async Task<IActionResult> RunString(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "org-stringtoken")] HttpRequest req
            , [Genesys] string token
            , ILogger log)
        {
            var org = await GetOrgAsync(token, "mypurecloud.ie");
            return new OkObjectResult(org);
        }

        [FunctionName("GetGenesysOrg")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "org")] HttpRequest req
            , [Genesys] IGenesysAccessToken token
            , ILogger log)
        {
            Organization org = await GetOrgAsync(token.Value, token.Environment);
            return new OkObjectResult(org);
        }

        [FunctionName("GetGenesysOrg2")]
        public async Task<IActionResult> GetGenesysOrg2(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "org2")] HttpRequest req
            , [Genesys(ClientSecret = "ClientSecret", ClientId = "ClientId", Environment = "Environment")] IGenesysAccessToken token
            , ILogger log)
        {
            Organization org = await GetOrgAsync(token.Value, token.Environment);
            return new OkObjectResult(org);
        }

        private async Task<Organization> GetOrgAsync(string token, string environment)
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
