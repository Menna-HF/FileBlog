public class CreatePostEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts", async (CreatePostRequest request, CreatePostHandler handler, CreatePostValidator validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (validation.IsValid)
            {
                var response = await handler.HandleAsync(request);
                return Results.Ok(response);
            }
            var errors = validation.Errors.Select(x => new { x.PropertyName, x.ErrorMessage });
            return Results.BadRequest(errors);
        });
    }
}