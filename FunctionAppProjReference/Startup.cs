//using Genesys.Azure.WebJobs.Extensions;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Hosting;
//using Microsoft.Extensions.DependencyInjection;

//[assembly: WebJobsStartup(typeof(FunctionAppProjReference.Startup))]
//namespace FunctionAppProjReference
//{
//    internal class Startup : IWebJobsStartup
//    {
//        public void Configure(IWebJobsBuilder builder)
//        {
//            //builder.Services.AddSingleton<GenesysTokenProvider>();
//            //builder.AddExtension<GenesysAttributeCustomConfigProvider>();
//            builder.AddExtension<GenesysAttributeConfigProvider>();
//        }
//    }
//}