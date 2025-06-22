public class GetAllPostsHandler
{
    private readonly IPostStorage _postStorage;
    public GetAllPostsHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<GetAllPostsResponse> HandleAsync()
    {
        return new GetAllPostsResponse
        {
            Posts = await _postStorage.GetAllPostsAsync(),
            Message = "The posts have been retrieved successfully."
        };
    }
}