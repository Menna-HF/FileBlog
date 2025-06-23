public class GetAllPostsHandler
{
    private readonly IPostStorage _postStorage;
    public GetAllPostsHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<GetAllPostsResponse> HandleAsync(GetAllPostsRequest request)
    {
        var posts = await _postStorage.GetFilteredPostsAsync(request.Tags, request.Categories);
        var message = (posts.Count > 0) ? "The posts have been retrieved successfully." : "No posts match the given tags or categories, please try other ones.";
        return new GetAllPostsResponse
            {
                Posts = posts,
                Message = message
            };
    }
}