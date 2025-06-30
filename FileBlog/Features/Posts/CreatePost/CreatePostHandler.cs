using System.Runtime.CompilerServices;
using System.Security.Claims;
public class CreatePostHandler
{
    private readonly IPostStorage _postStorage;

    public CreatePostHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<CreatePostResponse> HandleAsync(CreatePostRequest request, ClaimsPrincipal user)
    {
        var modifiedSlug = request.Slug?.Trim().ToLower().Replace(" ", "-") ?? request.Title.Trim().ToLower().Replace(" ", "-");
        if(await _postStorage.SlugExistsAsync(modifiedSlug))
            throw new Exception("Slug already exists, please try another one.");

        var author = user.Identity?.Name;
        if (string.IsNullOrEmpty(author))
            throw new Exception("No author is found.");

        var status = request.PublishingDate == null || request.PublishingDate > DateTime.UtcNow ? PostStatus.Draft : PostStatus.Published;

        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Author = author,
            Body = request.Body,
            Slug = modifiedSlug,
            Status = status,
            Tags = request.Tags,
            Categories = request.Categories,
            PublishingDate = request.PublishingDate ?? (status == PostStatus.Published ? DateTime.UtcNow : null),
            ModifiedDate = DateTime.UtcNow
        };
        await _postStorage.SavePostAsync(post);

        return new CreatePostResponse
        {
            Id = post.Id,
            Slug = post.Slug,
            Status = post.Status,
            PublishingDate = post.PublishingDate,
            Message = "The post has been created successfully."
        };
    }
}