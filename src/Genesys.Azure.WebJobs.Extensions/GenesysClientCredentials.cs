//MIT License
//Copyright(c) 2020 Noralogix
namespace Genesys.Azure.WebJobs.Extensions
{
    public class GenesysClientCredentials : IGenesysClientCredentials
    {
        public string OrgId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Environment { get; set; }
    }
}
