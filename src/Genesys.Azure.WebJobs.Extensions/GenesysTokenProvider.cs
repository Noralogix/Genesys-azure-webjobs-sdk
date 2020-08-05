using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysTokenContext
    {
        public CloudTable TokenTable { get; set; }
    }

    public class GenesysTokenProvider
    {
        private readonly IConfiguration _configuration;
        public GenesysTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual Task<IGenesysClientCredentials> GetClientCredentialsAsync()
        {
            IGenesysClientCredentials credentials = _configuration.GetSection(GenesysConfigNames.CredentialsSection).Get<GenesysClientCredentials>();
            return Task.FromResult(credentials);
        }

        public async Task<IGenesysAccessToken> GetTokenAsync(DateTime date, GenesysTokenContext tokenContext)
        {
            using (var client = new HttpClient())
            {
                var credentials = await GetClientCredentialsAsync();
                var token = await tokenContext.TokenTable.GetAuthTokenAsync(client, date, credentials);
                return token;
            }
        }
    }
}
