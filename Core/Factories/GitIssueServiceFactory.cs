using Core.Interfaces;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Factories
{
    public interface IGitIssueServiceFactory
    {
        IGitIssueService CreateService(GitServiceType serviceType);
    }

    public class GitIssueServiceFactory : IGitIssueServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<GitServiceType, Type> _serviceMappings = new()
        {
            { GitServiceType.GitHub, typeof(IGitHubIssueService) },
            { GitServiceType.GitLab, typeof(IGitLabIssueService) }
        };

        public GitIssueServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IGitIssueService CreateService(GitServiceType serviceType)
        {
            if (!_serviceMappings.TryGetValue(serviceType, out var serviceInterface))
            {
                throw new ArgumentOutOfRangeException(nameof(serviceType), $"Unsupported Git service type: {serviceType}");
            }

            return (IGitIssueService)_serviceProvider.GetRequiredService(serviceInterface);
            //return serviceType switch
            //{
            //    GitServiceType.GitHub => _serviceProvider.GetRequiredService<IGitHubIssueService>(),
            //    GitServiceType.GitLab => _serviceProvider.GetRequiredService<IGitLabIssueService>(),
            //    GitServiceType.Bitbucket => throw new NotImplementedException("Bitbucket service is not implemented yet."),
            //    _ => throw new ArgumentOutOfRangeException(nameof(serviceType), $"Unsupported Git service type: {serviceType}")
            //};
        }
    }
}