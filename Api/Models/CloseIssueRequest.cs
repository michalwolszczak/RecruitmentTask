using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public record CloseIssueRequest : BaseIssue
    {
        [Required]
        public string IssueId { get; set; }
    }
}