//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeValueBinder : IValueBinder
    {
        public Type Type => throw new NotImplementedException();

        private object _value;
        public GenesysAttributeValueBinder(object value)
        {
            _value = value;
        }

        public Task<object> GetValueAsync()
        {
            return Task.FromResult<object>(_value);
        }

        public Task SetValueAsync(object value, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public string ToInvokeString()
        {
            return "Genesys";
        }
    }
}
