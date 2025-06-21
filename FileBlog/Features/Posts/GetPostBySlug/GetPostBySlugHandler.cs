public class GetPostBySlugHandler
{
    private readonly IPostStorage _postStorage;

    public GetPostBySlugHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<GetPostBySlugResponse> HandleAsync(string slug)
    {
        slug = slug.Trim().ToLower().Replace(" ", "-");
        if (string.IsNullOrWhiteSpace(slug) || !await _postStorage.SlugExistsAsync(slug))
            throw new Exception("No post with the specified slug was found. Please try another one.");

        var post = await _postStorage.GetPostBySlugAsync(slug);
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
}