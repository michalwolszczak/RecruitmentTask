using Core.Models;

namespace Core.Interfaces
{
    public interface IGitIssueService
    {
        Task<Issue> CreateIssueAsync(string repositoryOwner, string repositoryName, string title, string description);
        Task<Issue> UpdateIssueAsync(string repositoryOwner, string repositoryName, string issueId, string title, string description);
        Task<Issue> CloseIssueAsync(string repositoryOwner, string repositoryName, string issueId);
    }
}