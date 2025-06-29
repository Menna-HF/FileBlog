using System.Runtime.CompilerServices;
public class CreatePostHandler
{
    private readonly IPostStorage _postStorage;

    public CreatePostHandler(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }
    public async Task<CreatePostResponse> HandleAsync(CreatePostRequest request)
    {
        var modifiedSlug = request.Slug?.Trim().ToLower().Replace(" ", "-") ?? request.Title.Trim().ToLower().Replace(" ", "-");
        if(await _postStorage.SlugExistsAsync(modifiedSlug))
            throw new Exception("Slug already exists, please try another one.");

        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Author = string.Empty, //  Will be handled later using JWT to extract the author's username
            Body = request.Body,
            Slug = modifiedSlug,
            Status = request.PublishingDate == null || request.PublishingDate > DateTime.UtcNow? PostStatus.Draft : PostStatus.Published,
            Tags = request.Tags,
            Categories = request.Categories,
            PublishingDate = request.PublishingDate,
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