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
    }
}
