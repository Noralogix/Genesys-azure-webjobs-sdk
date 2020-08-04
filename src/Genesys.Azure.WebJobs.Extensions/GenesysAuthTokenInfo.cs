//MIT License
//Copyright(c) 2020 Noralogix
namespace Genesys.Azure.WebJobs.Extensions
{
    internal class GenesysAuthTokenInfo
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int? ExpiresIn { get; set; }
        public string Error { get; set; }
    }
}
