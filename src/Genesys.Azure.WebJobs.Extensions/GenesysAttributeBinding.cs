//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeBinding : IBinding
    {
        private readonly string _parameterName;
        private readonly Type _parameterType;

        public GenesysAttributeBinding(string name, Type type)
        {
            _parameterName = name;
            _parameterType = type;
        }

        public bool FromAttribute => throw new NotImplementedException();

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
            => Task.FromResult((IValueProvider)new GenesysAttributeValueBinder(value));

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = _parameterName
            };
        }
    }
}
