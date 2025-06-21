public class CreatePostResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public PostStatus Status { get; set; } = PostStatus.Draft;
    public DateTime? PublishingDate { get; set; }
    public string Message { get; set; } = string.Empty;
}