using Core.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Settings
{
    public class GitServiceConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ApiToken { get; set; }

        public GitServiceConfiguration(IConfiguration configuration, GitServiceType gitServiceType)
        {
            ApiToken = GetApiTokenForService(configuration, gitServiceType);
            ApiBaseUrl = GetApiBaseUrlForService(configuration, gitServiceType);
        }

        private static string GetApiTokenForService(IConfiguration configuration, GitServiceType serviceType)
        {
            return serviceType switch
            {
                GitServiceType.GitHub => configuration["GitServices:GitHub:ApiToken"] ?? throw new ArgumentNullException("GitHub:ApiToken"),
                GitServiceType.GitLab => configuration["GitServices:GitLab:ApiToken"] ?? throw new ArgumentNullException("GitLab:ApiToken"),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceType))
            };
        }

        private static string GetApiBaseUrlForService(IConfiguration configuration, GitServiceType serviceType)
        {
            return serviceType switch
            {
                GitServiceType.GitHub => configuration["GitServices:GitHub:ApiBaseUrl"] ?? throw new ArgumentNullException("GitHub:ApiBaseUrl"),
                GitServiceType.GitLab => configuration["GitServices:GitLab:ApiBaseUrl"] ?? throw new ArgumentNullException("GitLab:ApiBaseUrl"),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceType))
            };
        }
    }
}