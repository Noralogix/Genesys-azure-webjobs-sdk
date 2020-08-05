//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public static class GenesysAzureAuth
    {
        public static async Task<IGenesysAccessToken> GetAuthTokenAsync(this CloudTable cloudTable, HttpClient apiClient
             , DateTime date
             , IGenesysClientCredentials clientCredentials
             , bool reload = false)
        {
            if (!reload)
            {
                try
                {
                    var existedToken = await cloudTable.GetAuthTokenAsync(clientCredentials);
                    if (existedToken?.ExpiresIn > date)
                    {
                        existedToken.Environment = clientCredentials.Environment;
                        return existedToken;
                    }
                }
                catch (Exception ex)
                {
                    throw new GenesysExtensionsException($"Cant load AccessToken from the Azure CloudTable '{cloudTable.Name}'.", ex);
                }
            }
            GenesysAuthTokenInfo authTokenInfo = null;
            
                authTokenInfo = await apiClient.GetTokenAsync(clientCredentials);
     
            if (authTokenInfo == null)
            {
                throw new GenesysExtensionsException($"Genesys API Token is null. ClientId '{clientCredentials.ClientId}'.");
            }
            else if (!string.IsNullOrEmpty(authTokenInfo.Error))
            {
                throw new GenesysExtensionsException($"Genesys API Token error. ClientId '{clientCredentials.ClientId}'. {authTokenInfo.Error}.");
            }

            var newToken = new GenesysAccessToken(clientCredentials)
            {
                Value = authTokenInfo.AccessToken,
                ExpiresIn = date.AddSeconds(authTokenInfo.ExpiresIn ?? 1000),
                ETag = "*"
            };

            await cloudTable.SetAuthTokenAsync(newToken);
            newToken.Environment = clientCredentials.Environment;
            return newToken;
        }
    }
}
