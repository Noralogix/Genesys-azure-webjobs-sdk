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
        public string TokenTable { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Azure Storage connection string.
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Genesys ClientId value.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Genesys ClientSecret value.
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// Gets or sets the app setting name that contains the Genesys Environment value.
        /// </summary>
        public string Environment { get; set; }
    }
}
