public interface IPostStorage
{
    public Task<bool> SlugExistsAsync(string slug);
    public Task SavePostAsync(Post post);
}

