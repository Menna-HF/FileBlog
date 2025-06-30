using System.Text.Json;
using Microsoft.VisualBasic;
public class FilePostStorage : IPostStorage
{
    private readonly string _postStorageFolder = Path.Combine("content", "posts");

    public FilePostStorage()
    {
        Directory.CreateDirectory(_postStorageFolder);
    }
    public string[] GetMetaFiles()
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
            if (!string.IsNullOrWhiteSpace(post?.Slug) && post.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase))
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
    public async Task<Post?> GetPostBySlugAsync(string slug)
    {
        var metaFiles = GetMetaFiles();
        foreach (var file in metaFiles)
        {
            var json = await File.ReadAllTextAsync(file);
            var post = JsonSerializer.Deserialize<Post>(json);
            if (!string.IsNullOrEmpty(post?.Slug) && post.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase))
                return post;
        }
        return null;
    }
    
    public async Task<bool> DeletePostAsync(string slug)
    {
        if (!await SlugExistsAsync(slug))
            return false;
        var post = await GetPostBySlugAsync(slug);
        string folderName = $"{(post?.PublishingDate ?? DateTime.UtcNow).ToString("yyyy-MM-dd")}-{slug}";
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