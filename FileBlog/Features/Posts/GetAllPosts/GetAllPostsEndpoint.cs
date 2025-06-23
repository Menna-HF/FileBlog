public class GetAllPostsEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/posts", async (GetAllPostsHandler handler, [AsParameters] GetAllPostsRequest request) =>
        {
            try
            {
                var response = await handler.HandleAsync(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
    });
    }
}