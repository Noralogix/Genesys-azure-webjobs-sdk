﻿using Genesys.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(SampleFunctionApp.Startup))]
namespace SampleFunctionApp
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<GenesysTokenProvider>();
            builder.Services.AddSingleton<GenesysAttributeBindingProvider>();
            builder.AddExtension<GenesysAttributeConfigProvider>();
        }
    }
}
