using System.Globalization;
using System.Text.Json;

public class FilePostStorage : IPostStorage
{
    private readonly string _postStorageFolder = Path.Combine("content", "posts");
    private readonly string _tagsStorageFolder = Path.Combine("content", "tags");
    private readonly string _categoriesStorageFolder = Path.Combine("content", "categories");
    private readonly string _url = "http://localhost:5054/";

    public FilePostStorage()
    {
        Directory.CreateDirectory(_postStorageFolder);
        Directory.CreateDirectory(_tagsStorageFolder);
        Directory.CreateDirectory(_categoriesStorageFolder);
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
        string url = $"{_url}posts/{slugInPath}";

        Directory.CreateDirectory(folderPath);

        string metaPath = Path.Combine(folderPath, "meta.json");
        string contentPath = Path.Combine(folderPath, "content.md");
        string assetsPath = Path.Combine(folderPath, "assets");

        Directory.CreateDirectory(assetsPath);

        var metaData = new
        {
            post.Title,
            post.Description,
            post.Tags,
            post.Categories,
            url
        };

        var metaJson = JsonSerializer.Serialize(metaData, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(metaPath, metaJson);
        await File.WriteAllTextAsync(contentPath, post.Body);

        foreach (var tag in post.Tags.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var modifiedTag = tag.Trim().ToLower().Replace(" ", "-");
            var tagPath = Path.Combine(_tagsStorageFolder, $"{modifiedTag}.json");

            if (!File.Exists(tagPath))
            {
                var tagJson = JsonSerializer.Serialize(new { Name = tag }, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(tagPath, tagJson);
            }
        }

        foreach (var category in post.Categories.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var modifiedCategory = category.Trim().ToLower().Replace(" ", "-");
            var categoryPath = Path.Combine(_categoriesStorageFolder, $"{modifiedCategory}.json");

            if (!File.Exists(categoryPath))
            {
                var categoryJson = JsonSerializer.Serialize(new { Name = category });
                await File.WriteAllTextAsync(categoryPath, categoryJson);
            }
        }
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
}