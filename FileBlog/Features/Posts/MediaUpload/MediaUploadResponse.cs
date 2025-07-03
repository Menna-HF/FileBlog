public class MediaUploadResponse
{
    public List<PostMediaInfo> MediaInfo { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}

public class PostMediaInfo
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set;} = string.Empty;
}