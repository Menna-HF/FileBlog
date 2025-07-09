public class CreatePostEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts", async (CreatePostRequest request, CreatePostHandler handler, CreatePostValidator validator, HttpContext httpContext) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(x => new { x.PropertyName, x.ErrorMessage });
                return Results.BadRequest(errors);
            }

            var response = await handler.HandleAsync(request, httpContext.User);
            return Results.Ok(response);

        }).RequireAuthorization("AllowedToCreatePosts");
    }
}