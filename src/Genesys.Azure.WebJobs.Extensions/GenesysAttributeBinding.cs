//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeBinding : IBinding
    {
        private const string AzureWebJobsStorage = "AzureWebJobsStorage";
        private const string AccessTokenTable = "GenesysAccessTokens";

        private readonly string _parameterName;
        private readonly Type _parameterType;
        private readonly GenesysTokenContext _tokenContext;

        private GenesysTokenProvider _tokenProvider;
        public bool FromAttribute => true;

        public GenesysAttributeBinding(string name, Type type, GenesysTokenProvider tokenProvider, GenesysTokenContext tokenContext)
        {
            _parameterName = name;
            _parameterType = type;
            _tokenProvider = tokenProvider;
            _tokenContext = tokenContext;
        }        

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
            => Task.FromResult((IValueProvider)new GenesysAttributeValueBinder(value));

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            var token = await _tokenProvider.GetTokenAsync(DateTime.UtcNow, _tokenContext);
            if (_parameterType == typeof(string)) return new GenesysAttributeValueBinder(token.Value);
            else return new GenesysAttributeValueBinder(token);
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = _parameterName, Type = _parameterType.Name
            };
        }
    }
}
