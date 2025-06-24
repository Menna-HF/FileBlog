public class DeletePostHandler
{
    private readonly IPostStorage _postStorage;
    public DeletePostHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<DeletePostResponse> HandleAsync(string slug)
    {
        if (!await _postStorage.DeletePostAsync(slug))
            throw new Exception("No post exists with the given slug.");

        return new DeletePostResponse
        {
            IsDeleted = true,
            Message = "Post deleted successfully."
        };
    }
}