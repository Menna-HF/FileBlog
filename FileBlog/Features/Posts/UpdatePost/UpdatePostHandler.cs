public class UpdatePostHandler
{
    private readonly IPostStorage _postStorage;
    public UpdatePostHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<UpdatePostResponse> HandleAsync(UpdatePostRequest request, string slug)
    {
        var post = await _postStorage.GetPostBySlugAsync(slug);
        if (post == null)
            throw new Exception("No post with the specified slug was found. Please try again.");
        if (!string.IsNullOrEmpty(request.Slug))
        {
            request.Slug = request.Slug.Trim().ToLower().Replace(" ", "-");
            if (request.Slug != post.Slug)
            {
                var slugExists = await _postStorage.SlugExistsAsync(request.Slug);
                if (!slugExists)
                {
                    post.Slug = request.Slug;
                    await _postStorage.DeletePostAsync(slug);
                }
                else
                    throw new Exception("Slug already exists, please try another one.");
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

        return new UpdatePostResponse
        {
            ModifiedPost = post,
            isUpdated = true,
            Message = "Post updated successfully. "
        };
        
    }
}