public class GetAllPostsHandler
{
    private readonly IPostStorage _postStorage;
    private readonly ILogger<GetAllPostsHandler> _logger;

    public GetAllPostsHandler(IPostStorage postStorage, ILogger<GetAllPostsHandler> logger)
    {
        _postStorage = postStorage;
        _logger = logger;
    }

    public async Task<GetAllPostsResponse> HandleAsync(GetAllPostsRequest request)
    {
        var posts = await GetFilteredPostsAsync(request.Tags, request.Categories);
        var message = (posts.Count > 0) ? "The posts have been retrieved successfully." : "No posts match the given tags or categories, please try other ones.";
        _logger.LogInformation("Retrieved posts after filtering.");

        return new GetAllPostsResponse
        {
            Posts = posts,
            Message = message
        };
    }

    private async Task<List<Post>> GetFilteredPostsAsync(List<string> tags, List<string> categories)
    {
        var allPosts = await _postStorage.GetAllPostsAsync();
        List<Post> filteredPosts = [];

        if (tags.Count == 0 && categories.Count == 0)
            return allPosts;

        foreach (var post in allPosts)
        {
            bool tagExists = tags.Count == 0 || post.Tags.Any(tag => tags.Contains(tag));
            bool categoryExists = categories.Count == 0 || post.Categories.Any(category => categories.Contains(category));

            if (tagExists && categoryExists)
                filteredPosts.Add(post);
        }
        return filteredPosts;
    }
}