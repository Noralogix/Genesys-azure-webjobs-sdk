//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TableStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeBindingProvider : IBindingProvider
    {
        private readonly IConfiguration _configuration;
        private readonly INameResolver _nameResolver;
        private readonly GenesysTokenProvider _tokenProvider;

        public GenesysAttributeBindingProvider(IConfiguration configuration, INameResolver resolver, GenesysTokenProvider tokenProvider)
        {
            _configuration = configuration;
            _nameResolver = resolver;
            _tokenProvider = tokenProvider;
        }

        public async Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameter = context.Parameter;
            var parameterName = context.Parameter.Name;
            var parameterType = context.Parameter.ParameterType;

            var attribute = parameter.GetCustomAttribute<GenesysAttribute>();
            if (attribute == null)
            {
                return null;
            }

            if (parameterType != typeof(string) && parameterType != typeof(IGenesysAccessToken))
            {
                throw new InvalidOperationException("Can't bind entity to type '" + parameterType + "'.");
            }

            var connectionString = GetConnectionString(attribute.Connection);
            if (!TableStorageAccount.TryParse(connectionString, out TableStorageAccount tableStorageAccount))
            {
                throw new InvalidOperationException($"Storage account connection string for '{IConfigurationExtensions.GetPrefixedConnectionStringName(attribute.Connection)}' is invalid");
            }

            var tableClient = tableStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(attribute.TokenTableName ?? GenesysConfigNames.GenesysTokensTable);
            await table.CreateIfNotExistsAsync();
            var tokenCtx = new GenesysTokenContext
            {
                TokenTable = table
            };

            IBinding binding = new GenesysAttributeBinding(parameterName, parameterType, _tokenProvider, tokenCtx);
            return binding;
        }

        private string GetConnectionString(string name) => _configuration.GetWebJobsConnectionString(_nameResolver.Resolve(name) ?? ConnectionStringNames.Storage);

    }
}
