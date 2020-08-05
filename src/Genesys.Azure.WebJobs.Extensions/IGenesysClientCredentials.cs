//MIT License
//Copyright(c) 2020 Noralogix
namespace Genesys.Azure.WebJobs.Extensions
{
    public interface IGenesysClientCredentials
    {
        string OrgId { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string Environment { get; }
    }

    internal static class GenesysClientCredentialsExt
    {
        internal static string GetOrgId(this IGenesysClientCredentials credentials) => credentials.OrgId ?? "";
    }
}
