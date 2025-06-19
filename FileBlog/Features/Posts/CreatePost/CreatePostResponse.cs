public class CreatePostResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public PostStatus Status { get; set; }
    public DateTime? PublishingDate { get; set; }
    public string Message { get; set; } = string.Empty;
}