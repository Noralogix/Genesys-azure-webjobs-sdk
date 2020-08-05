# genesys-azure-webjobs-sdk
Framework that simplifies the task of writing code that runs in Azure.

With the help of the binding attribute **[Genesys]**, you can simplify generation of the access token and keep it in the Azure Storage until expired.  After that you can use this token in the Genesys API.

        [FunctionName("GetGenesysOrg")]
        public static async Task<IActionResult> GetOrgAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "org")] HttpRequest req
            , [Genesys] IGenesysAccessToken token
            , ILogger log)
        {            
            var apiClient = new PureCloudPlatform.Client.V2.Client.ApiClient($"https://api.{token.Environment}");
            var configuration = new PureCloudPlatform.Client.V2.Client.Configuration(apiClient);
            configuration.AccessToken = token.Value;
            var orgApi = new PureCloudPlatform.Client.V2.Api.OrganizationApi(configuration);
            var org = await orgApi.GetOrganizationsMeAsync();            
            return new OkObjectResult(org);
        }
