public class GetAllPostsEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts/tagsAndCategories", async (GetAllPostsHandler handler, GetAllPostsRequest request) =>
        {
            var response = await handler.HandleAsync(request);
            return Results.Ok(response);
        });
    }
}