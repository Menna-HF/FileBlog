using System.Collections;

public class UpdatePostRequest
{
    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Body { get; set; }
    public PostStatus Status { get; set; } = PostStatus.Draft;
    public List<string>? Tags { get; set; }
    public List<string>? Categories { get; set; }
    public DateTime? PublishingDate { get; set; }
}