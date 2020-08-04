//MIT License
//Copyright(c) 2020 Noralogix
namespace Genesys.Azure.WebJobs.Extensions
{
    public interface IGenesysClientCredentials
    {
        public string OrgId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Environment { get; }
    }

    internal static class GenesysClientCredentialsExt
    {
        internal static string GetOrgId(this IGenesysClientCredentials credentials) => credentials.OrgId ?? "";
    }
}
