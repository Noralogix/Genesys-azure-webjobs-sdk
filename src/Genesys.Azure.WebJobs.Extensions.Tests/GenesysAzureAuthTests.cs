using Microsoft.Azure.Cosmos.Table;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions.Tests
{
    public class GenesysAzureAuthTests
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
        public async Task GetAuthTokenAsync()
        {
            var tableStorage = CloudStorageAccount.Parse(_config.AzureConnection);
            var tableClient = tableStorage.CreateCloudTableClient();
            var tokenTable = tableClient.GetTableReference("TestTokenTable");
            await tokenTable.CreateAsync();
            using (var client = new HttpClient())
            {
                var token = await tokenTable.GetAuthTokenAsync(client, DateTime.UtcNow, _config.Genesys);
                Assert.IsNotNull(token);
                Assert.IsNotNull(token.Value, "Token is empty");                
            }
            await tokenTable.DeleteAsync();
        }
    }
}
