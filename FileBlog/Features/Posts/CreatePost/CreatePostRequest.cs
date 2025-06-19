public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public DateTime? PublishingDate { get; set; }
}