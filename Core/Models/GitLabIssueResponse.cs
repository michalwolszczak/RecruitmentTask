namespace Core.Models
{
    public record GitLabIssueResponse
    {
        public int Id { get; set; }
        public int Iid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
