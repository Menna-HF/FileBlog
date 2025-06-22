public interface IPostStorage
{
    public Task<bool> SlugExistsAsync(string slug);
    public Task SavePostAsync(Post post);
    public Task<Post> GetPostBySlugAsync(string slug);
    public Task<List<Post>> GetAllPostsAsync();
}

