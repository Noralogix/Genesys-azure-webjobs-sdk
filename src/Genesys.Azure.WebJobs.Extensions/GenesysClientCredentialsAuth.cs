//MIT License
//Copyright(c) 2020 Noralogix
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public static class GenesysClientCredentialsAuth
    {
        public static async Task<GenesysAuthTokenInfo> GetTokenAsync(this HttpClient httpClient, IGenesysClientCredentials clientCredentials)
        {
            var path = "/oauth/token";

            var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes($"{clientCredentials.ClientId}:{clientCredentials.ClientSecret}"));
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.{clientCredentials.Environment}" + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });
            request.Content = content;
            var response = await httpClient.SendAsync(request);

            try
            {
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var authTokenInfo = JsonSerializer.Deserialize<GenesysAuthTokenInfo>(responseContent);
                return authTokenInfo;
            }
            catch (HttpRequestException ex)
            {
                int statusCode = (int)response.StatusCode;
                throw new GenesysExtensionsException($"Error calling Genesys API PostToken. Status Code {statusCode}. " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new GenesysExtensionsException($"GetTokenAsync error. " + ex.Message);
            }
        }
    }
}
