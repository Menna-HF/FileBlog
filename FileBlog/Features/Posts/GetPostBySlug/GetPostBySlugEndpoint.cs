public class GetPostBySlugEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/posts/{slug}", async (GetPostBySlugHandler handler, string slug) =>
        {
            var response = await handler.HandleAsync(slug);
            return Results.Ok(response);
        });
    }
}