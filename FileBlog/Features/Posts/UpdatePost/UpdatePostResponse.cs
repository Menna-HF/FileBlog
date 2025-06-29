public class UpdatePostResponse
{
    public Post? ModifiedPost { get; set; }
    public bool isUpdated { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}