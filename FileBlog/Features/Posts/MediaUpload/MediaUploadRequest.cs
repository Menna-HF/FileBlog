public class MediaUploadRequest
{
    public List<IFormFile> Files { get; set; } = [];
    public string Slug { get; set; } = string.Empty;
}