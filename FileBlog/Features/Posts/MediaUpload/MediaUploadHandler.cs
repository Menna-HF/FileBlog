public class MediaUploadHandler
{
    private readonly IPostStorage _postStorage;
    private readonly ILogger<MediaUploadHandler> _logger;

    public MediaUploadHandler(IPostStorage postStorage, ILogger<MediaUploadHandler> logger)
    {
        _postStorage = postStorage;
        _logger = logger;
    }
    /*public async Task<MediaUploadResponse> HandleAsync(MediaUploadRequest request)
    {
        var slug = request.Slug.Trim().ToLower().Replace(" ", "-");
        if (!await _postStorage.SlugExistsAsync(slug))
        {
            _logger.LogWarning("The slug '{slug}' doesn't exist.", slug);
            throw new Exception("Post not found, please try again.");
        }

        var slugPostFolder = string.Empty;
        var postsFolder = Directory.GetDirectories(Path.Combine("content", "posts"));

        foreach (var folder in postsFolder)
        {
            var folderName = Path.GetFileName(folder);
            if (folderName.EndsWith($"-{slug}") == true)
            {
                slugPostFolder = folder;
                break;
            }
        }

        string assetsPath = Path.Combine(slugPostFolder, "assets");
        Directory.CreateDirectory(assetsPath);
    }*/
}