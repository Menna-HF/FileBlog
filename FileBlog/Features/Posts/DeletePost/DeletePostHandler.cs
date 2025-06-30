public class DeletePostHandler
{
    private readonly IPostStorage _postStorage;
    private readonly ILogger<DeletePostHandler> _logger;
    public DeletePostHandler(IPostStorage postStorage, ILogger<DeletePostHandler> logger)
    {
        _postStorage = postStorage;
        _logger = logger;
    }
    public async Task<DeletePostResponse> HandleAsync(string slug)
    {
        try
        {
            if (!await _postStorage.DeletePostAsync(slug))
            {
                _logger.LogWarning("Failed to find a post with the slug '{slug}'", slug);
                throw new Exception("No post exists with the given slug.");
            }
            _logger.LogInformation("Post with the slug '{slug}' deleted successfully.", slug);

            return new DeletePostResponse
            {
                IsDeleted = true,
                Message = "Post deleted successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while deleting the post.");
            throw;
        }
    }
}