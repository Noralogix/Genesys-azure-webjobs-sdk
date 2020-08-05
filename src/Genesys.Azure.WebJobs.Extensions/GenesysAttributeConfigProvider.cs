//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeConfigProvider : IExtensionConfigProvider
    {        
        public readonly GenesysAttributeBindingProvider _bindingProvider;
        public GenesysAttributeConfigProvider(GenesysAttributeBindingProvider bindingProvider) =>
            _bindingProvider = bindingProvider;

        public void Initialize(ExtensionConfigContext context) => context
                .AddBindingRule<GenesysAttribute>()
                .Bind(_bindingProvider);
    }
}
