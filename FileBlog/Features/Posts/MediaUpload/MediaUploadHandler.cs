using FileBlog.Features.ExceptionHandling;

public class MediaUploadHandler
{
    private readonly IPostStorage _postStorage;
    private readonly ILogger<MediaUploadHandler> _logger;

    public MediaUploadHandler(IPostStorage postStorage, ILogger<MediaUploadHandler> logger)
    {
        _postStorage = postStorage;
        _logger = logger;
    }

    public async Task<MediaUploadResponse> HandleAsync(MediaUploadRequest request)
    {
        var slug = request.Slug.Trim().ToLower().Replace(" ", "-");

        if (!await _postStorage.SlugExistsAsync(slug))
        {
            _logger.LogError("The slug '{slug}' doesn't exist.", slug);
            throw new NoPostFoundException("Post not found, please try again.");
        }

        var mediaPath = PostMediaPath(slug);
        if (string.IsNullOrEmpty(mediaPath))
        {
            _logger.LogError("Media path is not found.");
            throw new NotFoundException("Media path can't be found, please try again.");
        }

        Directory.CreateDirectory(mediaPath);
        List<PostMediaInfo> mediaInfos = await SaveMediaFiles(request.Files, mediaPath);
        _logger.LogInformation("Saved media files in '{mediaPath}' successfully.", mediaPath);

        return new MediaUploadResponse
        {
            MediaInfo = mediaInfos,
            Message = "Media uploaded successfully."
        };
    }

    private string PostMediaPath(string slug)
    {
        var savedFolders = Directory.GetDirectories("content/posts");

        foreach (var folder in savedFolders)
        {
            if (folder.EndsWith(slug))
            {
                _logger.LogInformation("Found post folder for slug '{slug}' at {folder}", slug, folder);
                return Path.Combine(folder, "assets");
            }
        }
        return string.Empty;
    }

    private async Task<List<PostMediaInfo>> SaveMediaFiles(List<IFormFile> mediaFiles, string mediaPath)
    {
        List<PostMediaInfo> mediaInfos = [];

        foreach (var file in mediaFiles)
        {
            var fileName = file.FileName.Replace(" ", "-");
            var storeFilePath = Path.Combine(mediaPath, fileName);
            mediaInfos.Add(new PostMediaInfo
            {
                FileName = fileName,
                FilePath = storeFilePath
            });
            await using (var stream = new FileStream(storeFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        return mediaInfos;
    }
}