namespace Core.Models
{
    public class Issue
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryOwner { get; set; }
    }
}