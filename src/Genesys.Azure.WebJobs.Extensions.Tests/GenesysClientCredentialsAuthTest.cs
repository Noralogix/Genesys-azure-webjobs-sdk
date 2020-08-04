using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Genesys.Azure.WebJobs.Extensions;
namespace Genesys.Azure.WebJobs.Extensions.Tests
{
    public class GenesysClientCredentialsAuthTests
    {
        private const string _configFileName = "config.json";
        private Config _config;

        [SetUp]
        public void Setup()
        {
            var jsonConfigFile = File.ReadAllText(_configFileName);
            _config = JsonSerializer.Deserialize<Config>(jsonConfigFile);
        }

        [Test]
        public async Task GetTokenAsync()
        {
            using (var client = new HttpClient())
            {
               var token = await client.GetTokenAsync(_config.Genesys);                
                Assert.IsNotNull(token);
                Assert.IsNull(token.Error, token.Error);
                Assert.IsTrue(!string.IsNullOrEmpty(token.AccessToken), "AccessToken is empty");
                Assert.IsTrue(token.ExpiresIn.HasValue, "ExpiresIn is not specified");
            }            
        }

        //[Test]
        //public void GetTokenAsyncFailedWithError()
        //{
        //    var jsonConfigFile = File.ReadAllText("configFailed.json");
        //    _config = JsonSerializer.Deserialize<Config>(jsonConfigFile);
          
        //    using (var client = new HttpClient())
        //    {
        //        Assert.Throws<GenesysExtensionsException>(async () => await client.GetTokenAsync(_config.Genesys));
        //    }
        //}
    }
}