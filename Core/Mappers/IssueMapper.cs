using Core.Models;

namespace Core.Mappers
{
    public static class IssueMapper
    {
        public static Issue MapToIssue(GitHubIssueResponse gitHubIssueResponse, string repositoryOwner, string repositoryName) =>
            new()
            {
                Id = gitHubIssueResponse.Number.ToString(),
                Title = gitHubIssueResponse.Title,
                Description = gitHubIssueResponse.Body,
                RepositoryName = repositoryOwner,
                RepositoryOwner = repositoryOwner
            };

        public static Issue MapToIssue(GitLabIssueResponse gitLabIssueResponse, string repositoryOwner, string repositoryName) =>
            new()
            {
                Id = gitLabIssueResponse.Iid.ToString(),
                Title = gitLabIssueResponse.Title,
                Description = gitLabIssueResponse.Description,
                RepositoryName = repositoryOwner,
                RepositoryOwner = repositoryOwner
            };        
    }
}