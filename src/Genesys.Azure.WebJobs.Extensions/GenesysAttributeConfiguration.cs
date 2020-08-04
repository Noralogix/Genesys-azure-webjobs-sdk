//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs.Host.Config;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeConfiguration : IExtensionConfigProvider
    {
        public readonly GenesysAttributeBindingProvider _bindingProvider;
        public GenesysAttributeConfiguration(GenesysAttributeBindingProvider bindingProvider) =>
            _bindingProvider = bindingProvider;

        public void Initialize(ExtensionConfigContext context) => context
                .AddBindingRule<GenesysAttribute>()
                .Bind(_bindingProvider);
    }
}
