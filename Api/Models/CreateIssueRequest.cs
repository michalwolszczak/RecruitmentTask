using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public record CreateIssueRequest : BaseIssue
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }        
}