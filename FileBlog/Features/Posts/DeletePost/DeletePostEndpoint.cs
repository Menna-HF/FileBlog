public class DeletePostEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete("/posts/{slug}", async (DeletePostHandler handler, string slug) =>
        {
            var response = await handler.HandleAsync(slug);
            return Results.Ok(response);
                
        }).RequireAuthorization("AllowedToDeletePosts");
    }
}