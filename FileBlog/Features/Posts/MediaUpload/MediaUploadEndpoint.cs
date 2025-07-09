public class MediaUploadEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts/media", async (MediaUploadValidator validator, MediaUploadRequest request, MediaUploadHandler handler) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(x => new { x.PropertyName, x.ErrorMessage });
                return Results.BadRequest(errors);
            }

            var response = await handler.HandleAsync(request);
            return Results.Ok(response);
            
        }).RequireAuthorization("AllowedToCreatePosts");
    }
}