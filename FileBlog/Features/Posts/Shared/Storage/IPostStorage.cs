public interface IPostStorage
{
    public string[] GetMetaFiles();
    public Task<bool> SlugExistsAsync(string slug);
    public Task SavePostAsync(Post post);
    public Task<Post?> GetPostBySlugAsync(string slug);
    public Task<bool> DeletePostAsync(string slug);
}

