using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public record BaseIssue
    {
        [Required]
        public GitServiceType ServiceType { get; set; }
        [Required]
        public string RepositoryOwner { get; set; }
        [Required]
        public string RepositoryName { get; set; }
    }
}