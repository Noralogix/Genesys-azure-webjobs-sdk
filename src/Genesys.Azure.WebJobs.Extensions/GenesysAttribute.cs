//MIT License
//Copyright(c) 2020 Noralogix
using Microsoft.Azure.WebJobs.Description;
using System;

namespace Genesys.Azure.WebJobs.Extensions
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public class GenesysAttribute : Attribute
    {
        /// <summary>
        /// Get or sets Genesys TokenTable where tokens will be cached.
        /// </summary>
        [AutoResolve]
        public string TokenTable { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Azure Storage connection string.
        /// </summary>
        [AppSetting(Default = "AzureWebJobsStorage")]
        public string Connection { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Genesys ClientId value.
        /// </summary>
        [AppSetting(Default = "GenesysClientId")]
        public string ClientId { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Genesys ClientSecret value.
        /// </summary>
        [AppSetting(Default = "GenesysClientSecret")]
        public string ClientSecret { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Genesys Environment value.
        /// </summary>
        [AppSetting(Default = "GenesysEnvironment")]
        public string Environment { get; set; }
    }
}
