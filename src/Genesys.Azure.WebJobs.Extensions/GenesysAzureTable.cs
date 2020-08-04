//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    internal static class GenesysAzureTable
    {
        public static async Task<IGenesysAccessToken> GetAuthTokenAsync(this CloudTable cloudTable, IGenesysClientCredentials settings)
        {
            var q = new TableQuery<GenesysAccessToken>()
                .Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, settings.GetOrgId()),
                    TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, settings.ClientId)))
                .Take(1);

            var res = await cloudTable.ExecuteQuerySegmentedAsync(q, null);
            return res.FirstOrDefault();
        }

        public static Task SetAuthTokenAsync(this CloudTable cloudTable, GenesysAccessToken accessToken)
        {
            return cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(accessToken));
        }
    }
}
