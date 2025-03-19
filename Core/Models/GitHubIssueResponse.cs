namespace Core.Models
{
    public record GitHubIssueResponse 
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}