using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Genesys.Azure.WebJobs.Extensions.GenesysWebJobsStartup))]
namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<GenesysAttributeConfigProvider>();
        }
    }
}