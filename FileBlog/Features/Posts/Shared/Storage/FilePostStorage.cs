using System.Text.Json;
using Microsoft.VisualBasic;
public class FilePostStorage : IPostStorage
{
    private readonly string _postStorageFolder = Path.Combine("content", "posts");

    public FilePostStorage()
    {
        Directory.CreateDirectory(_postStorageFolder);
    }
    private string[] GetMetaFiles()
    {
        return Directory.GetFiles(_postStorageFolder, "meta.json", SearchOption.AllDirectories);
    }
    public async Task<bool> SlugExistsAsync(string slug)
    {
        var metaFiles = GetMetaFiles();
        foreach (var file in metaFiles)
        {
            var json = await File.ReadAllTextAsync(file);
            var post = JsonSerializer.Deserialize<Post>(json);
            if (!string.IsNullOrWhiteSpace(post?.Slug) && post.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase) == true)
                return true;
        }
        return false;
    }
    public async Task SavePostAsync(Post post)
    {
        string dateInPath = (post.PublishingDate ?? DateTime.UtcNow).ToString("yyyy-MM-dd");
        string slugInPath = post.Slug.Trim().ToLower().Replace(" ", "-");
        string folderName = $"{dateInPath}-{slugInPath}";
        string folderPath = Path.Combine(_postStorageFolder, folderName);
        Directory.CreateDirectory(folderPath);
        string metaPath = Path.Combine(folderPath, "meta.json");
        string contentPath = Path.Combine(folderPath, "content.md");
        string assetsPath = Path.Combine(folderPath, "assets");
        Directory.CreateDirectory(assetsPath);

        var json = JsonSerializer.Serialize(post, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(metaPath, json);
        await File.WriteAllTextAsync(contentPath, post.Body);
    }
    public async Task<Post> GetPostBySlugAsync(string slug)
    {
        var metaFiles = Directory.GetFiles(_postStorageFolder, "meta.json", SearchOption.AllDirectories);
        foreach (var file in metaFiles)
        {
            var json = await File.ReadAllTextAsync(file);
            var post = JsonSerializer.Deserialize<Post>(json);
            if (!string.IsNullOrEmpty(post?.Slug) && post.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase))
                return post;
        }
        return null;
    }
    public async Task<List<Post>> GetAllPostsAsync()
    {
        List<Post> allPosts = [];
        var metaFiles = GetMetaFiles();
        foreach (var file in metaFiles)
        {
            var json = await File.ReadAllTextAsync(file);
            var post = JsonSerializer.Deserialize<Post>(json);
            if (post != null)
                allPosts.Add(post);
        }
        return allPosts;
    }
    public async Task<List<Post>> GetFilteredPostsAsync(List<string> tags, List<string> categories)
    {
        var allPosts = await GetAllPostsAsync();
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
    public async Task<bool> DeletePostAsync(string slug)
    {
        if (!await SlugExistsAsync(slug))
            return false;
        var post = await GetPostBySlugAsync(slug);
        string folderName = $"{(post.PublishingDate ?? DateTime.UtcNow).ToString("yyyy-MM-dd")}-{slug}";
        string folderPath = Path.Combine(_postStorageFolder, folderName);
        try
        {
            Directory.Delete(folderPath, true);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}