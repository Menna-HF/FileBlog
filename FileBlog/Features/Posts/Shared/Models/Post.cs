public class Post
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public PostStatus Status { get; set; } = PostStatus.Draft;
    public List<string> Tags { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public DateTime? PublishingDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
public enum PostStatus
{
    Draft,
    Published
}