using Core.Models;

namespace Api.Models
{
    public record IssueResponse : BaseIssue
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static IssueResponse? FromIssue(Issue issue, GitServiceType serviceType)
        {
            if (issue == null)
                return null;

            return new IssueResponse
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                RepositoryName = issue.RepositoryName,
                RepositoryOwner = issue.RepositoryOwner,
                ServiceType = serviceType
            };
        }
    }
}