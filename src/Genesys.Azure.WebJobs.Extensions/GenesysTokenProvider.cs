using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysTokenContext
    {
        public string Environment { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public CloudTable TokenTable { get; set; }
    }

    public class GenesysTokenProvider
    {
        private readonly IConfiguration _configuration;
        public GenesysTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual Task<IGenesysClientCredentials> GetClientCredentialsAsync(GenesysTokenContext tokenContext)
        {
            IGenesysClientCredentials credentials = new GenesysClientCredentials
            {
                ClientId = _configuration[tokenContext.ClientId ?? GenesysConfigNames.ClientId],
                ClientSecret = _configuration[tokenContext.ClientSecret ?? GenesysConfigNames.ClientSecret],
                Environment = _configuration[tokenContext.Environment ?? GenesysConfigNames.Environment],
                OrgId = _configuration[GenesysConfigNames.OrgId],
            };
            //_configuration.GetSection(GenesysConfigNames.CredentialsSection).Get<GenesysClientCredentials>();

            return Task.FromResult(credentials);
        }

        public static string[] Environments = new string[] {
            "mypurecloud.ie", "mypurecloud.com", "mypurecloud.de", "mypurecloud.jp",
            "usw2.pure.cloud", "cac1.pure.cloud", "euw2.pure.cloud", "apne2.pure.cloud", "mypurecloud.com.au"
        };

        private void ValidateEnvironment(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                throw new ArgumentException("Environment is not defined.");
            if (!Environments.Any(env => env.Equals(environment, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Wrong Environment.");
        }

        public virtual async Task<IGenesysAccessToken> GetTokenAsync(DateTime date
            , GenesysTokenContext tokenContext
            , IReadOnlyDictionary<string, object> webjobBindingData)
        {
            var credentials = await GetClientCredentialsAsync(tokenContext);

            if (credentials == null)
                throw new ArgumentNullException("Cant get token with nullable client credentials.");
            if (string.IsNullOrEmpty(credentials.ClientId))
                throw new ArgumentException("ClientId is not defined.");
            if (string.IsNullOrEmpty(credentials.ClientSecret))
                throw new ArgumentException("ClientSecret is not defined.");
            ValidateEnvironment(credentials.Environment);

            using (var client = new HttpClient())
            {
                var token = await tokenContext.TokenTable.GetAuthTokenAsync(client, date, credentials);
                return token;
            }
        }
    }
}
