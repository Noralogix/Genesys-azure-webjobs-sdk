//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;

namespace Genesys.Azure.WebJobs.Extensions
{
    [Extension("GenesysAzureStorageCustom", "Genesys")]
    public class GenesysAttributeCustomConfigProvider : IExtensionConfigProvider
    {
        private readonly GenesysAttributeBindingProvider _bindingProvider;
        public GenesysAttributeCustomConfigProvider(IConfiguration configuration, INameResolver resolver, GenesysTokenProvider tokenProvider) =>
            _bindingProvider = new GenesysAttributeBindingProvider(configuration, resolver, tokenProvider);

        public void Initialize(ExtensionConfigContext context) => context
                .AddBindingRule<GenesysAttribute>()
                .Bind(_bindingProvider);
    }

    [Extension("GenesysAzureStorage", "Genesys")]
    public class GenesysAttributeConfigProvider : IExtensionConfigProvider
    {        
        private readonly GenesysAttributeBindingProvider _bindingProvider;
        public GenesysAttributeConfigProvider(IConfiguration configuration, INameResolver resolver) =>
            _bindingProvider = new GenesysAttributeBindingProvider(configuration, resolver, new GenesysTokenProvider(configuration));

        public void Initialize(ExtensionConfigContext context) => context
                .AddBindingRule<GenesysAttribute>()
                .Bind(_bindingProvider);
    }
}
