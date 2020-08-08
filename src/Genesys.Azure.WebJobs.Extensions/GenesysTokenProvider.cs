using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
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

        public virtual async Task<IGenesysAccessToken> GetTokenAsync(DateTime date, GenesysTokenContext tokenContext)
        {
            var credentials = await GetClientCredentialsAsync(tokenContext);

            if (credentials == null)
                throw new ArgumentNullException("Cant get Genesys Token with nullable client credentials.");
            if (string.IsNullOrEmpty(credentials.ClientId))
                throw new ArgumentException("Genesys ClientId is not defined.");
            if (string.IsNullOrEmpty(credentials.ClientSecret))
                throw new ArgumentException("Genesys ClientSecret is not defined.");
            if (string.IsNullOrEmpty(credentials.Environment))
                throw new ArgumentException("Genesys Environment is not defined.");

            using (var client = new HttpClient())
            {
                var token = await tokenContext.TokenTable.GetAuthTokenAsync(client, date, credentials);
                return token;
            }
        }
    }
}
