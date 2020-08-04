//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs.Host.Bindings;
using System.Reflection;
using System.Threading.Tasks;

namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysAttributeBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameter = context.Parameter;

            var bindingAttribute = parameter.GetCustomAttribute<GenesysAttribute>();
            if (bindingAttribute == null)
            {
                return null;
            }

            IBinding binding = new GenesysAttributeBinding(context.Parameter.Name, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }
    }
}
