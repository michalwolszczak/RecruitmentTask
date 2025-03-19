using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public record UpdateIssueRequest : BaseIssue
    {
        [Required]
        public string IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}