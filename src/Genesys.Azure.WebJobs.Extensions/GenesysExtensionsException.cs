//MIT License
//Copyright(c) 2020 Noralogix
using System;
namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysExtensionsException : Exception
    {
        public GenesysExtensionsException(string message, Exception innerException = null) : base(message, innerException)
        {

        }
    }
}
