using FileBlog.Features.ExceptionHandling;

public class UpdatePostHandler
{
    private readonly IPostStorage _postStorage;
    private readonly ILogger<UpdatePostHandler> _logger;

    public UpdatePostHandler(IPostStorage postStorage, ILogger<UpdatePostHandler> logger)
    {
        _postStorage = postStorage;
        _logger = logger;
    }

    public async Task<UpdatePostResponse> HandleAsync(UpdatePostRequest request, string slug)
    {
        var post = await _postStorage.GetPostBySlugAsync(slug);

        if (post == null)
        {
            _logger.LogWarning("The post with slug '{slug}' can't be found.", slug);
            throw new NoPostFoundException("No post with the specified slug was found. Please try again.");
        }

        if (!string.IsNullOrEmpty(request.Slug))
        {
            request.Slug = request.Slug.Trim().ToLower().Replace(" ", "-");

            if (request.Slug != post.Slug)
            {
                var slugExists = await _postStorage.SlugExistsAsync(request.Slug);

                if (slugExists)
                {
                    _logger.LogWarning("The slug '{slug}' already exists.", request.Slug);
                    throw new SlugExistsException("Slug already exists, please try another one.");
                }

                post.Slug = request.Slug;
                await _postStorage.DeletePostAsync(slug);
                _logger.LogInformation("Replaced slug '{oldSlug}' with '{newSlug}'.", slug, request.Slug);
            }
        }
        if (!string.IsNullOrEmpty(request.Title))
            post.Title = request.Title.Trim();
        if (!string.IsNullOrEmpty(request.Description))
            post.Description = request.Description.Trim();
        if (!string.IsNullOrEmpty(request.Body))
            post.Body = request.Body;
        if (request.Tags != null)
            post.Tags = request.Tags;
        if (request.Categories != null)
            post.Categories = request.Categories;
        if (request.Status == PostStatus.Published)
        {
            post.Status = PostStatus.Published;
            post.PublishingDate = DateTime.UtcNow;
        }
        else if (request.Status == PostStatus.Draft && request.PublishingDate != null)
            post.PublishingDate = request.PublishingDate;

        post.ModifiedDate = DateTime.UtcNow;
        await _postStorage.SavePostAsync(post);

        _logger.LogInformation("Post with the slug '{slug}' updated successfully.", post.Slug);

        return new UpdatePostResponse
        {
            ModifiedPost = post,
            isUpdated = true,
            Message = "Post updated successfully."
        };
    }
}