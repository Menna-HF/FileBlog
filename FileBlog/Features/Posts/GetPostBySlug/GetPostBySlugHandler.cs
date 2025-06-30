public class GetPostBySlugHandler
{
    private readonly IPostStorage _postStorage;
    private readonly ILogger<GetPostBySlugHandler> _logger;

    public GetPostBySlugHandler(IPostStorage postStorage, ILogger<GetPostBySlugHandler> logger)
    {
        _postStorage = postStorage;
        _logger = logger;
    }
    public async Task<GetPostBySlugResponse> HandleAsync(string slug)
    {
        try
        {
            slug = slug.Trim().ToLower().Replace(" ", "-");
            if (string.IsNullOrWhiteSpace(slug) || !await _postStorage.SlugExistsAsync(slug))
            {
                _logger.LogWarning("The post with slug '{slug}' can't be found.", slug);
                throw new Exception("No post with the specified slug was found. Please try another one.");
            }

            var post = await _postStorage.GetPostBySlugAsync(slug);
            _logger.LogInformation("Successfully retrieved post with slug '{slug}'.", slug);

            return new GetPostBySlugResponse
            {
                Title = post.Title,
                Description = post.Description,
                Author = post.Author,
                Body = post.Body,
                Tags = post.Tags,
                Categories = post.Categories,
                PublishingDate = post.PublishingDate,
                Message = "The post has been retrieved successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during while retrieving post with the slug '{slug}'.", slug);
            throw;
        }
    }
}