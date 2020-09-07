//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using System;

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

        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<GenesysAttribute>();
            rule.AddValidator(ValidateBinding);
            rule.Bind(_bindingProvider);
        }

        private void ValidateBinding(GenesysAttribute attribute, Type type)
        {
            if (string.IsNullOrEmpty(attribute.ClientId))
                throw new ArgumentNullException("Gengesys ClientId is missing");
            if (string.IsNullOrEmpty(attribute.ClientSecret))
                throw new ArgumentNullException("Gengesys ClientSecret is missing");
            if (string.IsNullOrEmpty(attribute.Environment))
                throw new ArgumentNullException("Gengesys Environment is missing");
            if (string.IsNullOrEmpty(attribute.Connection))
                throw new ArgumentNullException("Gengesys Connection is missing");
        }
    }
}
