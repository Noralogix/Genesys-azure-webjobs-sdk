# Genesys.Azure.WebJobs.Extensions .NET [![NuGet version](https://badge.fury.io/nu/Genesys.Azure.WebJobs.Extensions.svg)](https://www.nuget.org/packages/Genesys.Azure.WebJobs.Extensions) [![Nuget downloads](https://img.shields.io/nuget/dt/Genesys.Azure.WebJobs.Extensions)](https://www.nuget.org/packages/Genesys.Azure.WebJobs.Extensions)

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
        ...
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
            builder.AddExtension<GenesysAttributeConfigProvider>();
        }
    }
}
```
Azure Function Config file. GenesysOrgId For Azure Token Table partition key, otherwise partition key value will be empty string.
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

GenesysAtribute properies
- Connection Optional. Azure storage account to use. Default connect AzureWebJobsStorage.
- TokenTable Optional. Azure Table name, where tokens will be cached. Default table name GenesysTokens.
- ClientId Optional. The App setting name that contains the Genesys ClientId value. Default name GenesysClientId.
- ClientSecret Optional. The app setting name that contains the Genesys ClientSecret value. Default name - GenesysClientSecret.
- Environment Optional. The app setting name that contains the Genesys Environment value. Default name GenesysEnvironment.

Also you can specify your custom GenesysTokenProvider with GenesysAttributeCustomConfigProvider
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
            builder.AddExtension<GenesysAttributeCustomConfigProvider>();
        }
    }
}
```


