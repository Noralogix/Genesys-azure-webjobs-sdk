# genesys-azure-webjobs-sdk
Framework that simplifies the task of writing code that runs in Azure.

```{"language":"csharp"}
Install-Package Genesys.Azure.WebJobs.Extensions
```
With the help of the binding attribute **[Genesys]**, you can simplify generation of the access token and keep it in the Azure Storage until expired.  After that you can use this token in the Genesys API.
Authorization based on client credentials specified in the Azure App Function config.
Library namespace
```{"language":"csharp"}
using Genesys.Azure.WebJobs.Extensions;
```
Azure Function with binding attribute **[Genesys]**
```{"language":"csharp"}
namespace SampleFunctionApp
{
    public static class GetGenesysOrg
    {
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
```

Specify binding extension in the Azure Function Startup class
```{"language":"csharp"}
using Genesys.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(SampleFunctionApp.Startup))]
namespace SampleFunctionApp
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<GenesysTokenProvider>();
            builder.Services.AddSingleton<GenesysAttributeBindingProvider>();
            builder.AddExtension<GenesysAttributeConfigProvider>();
        }
    }
}
```
Azure Function Config file:
```{"language":"csharp"}
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "GenesysClientId": "",
    "GenesysClientSecret": "",
    "GenesysEnvironment": "mypurecloud.ie",
    "GenesysOrgId": ""
  }
}
```
