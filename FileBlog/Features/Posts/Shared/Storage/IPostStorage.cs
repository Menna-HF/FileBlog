public interface IPostStorage
{
    public Task<bool> SlugExistsAsync(string slug);
    public Task SavePostAsync(Post post);
    public Task<Post?> GetPostBySlugAsync(string slug);
    public Task<List<Post>> GetAllPostsAsync();
    public Task<List<Post>> GetFilteredPostsAsync(List<string> tags, List<string> categories);
    public Task<bool> DeletePostAsync(string slug);
}

