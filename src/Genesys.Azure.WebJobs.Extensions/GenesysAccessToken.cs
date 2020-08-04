//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.Cosmos.Table;
using System;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAccessToken : TableEntity, IGenesysAccessToken
    {
        public string Value { get; set; }
        public DateTime ExpiresIn { get; set; }

        public GenesysAccessToken()
        {
        }
        public GenesysAccessToken(IGenesysClientCredentials settings)
        {
            PartitionKey = settings.GetOrgId();
            RowKey = settings.ClientId;
        }
    }

    public interface IGenesysAccessToken
    {
        string Value { get; }
    }
}
