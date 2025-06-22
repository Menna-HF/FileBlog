public class GetAllPostsEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/posts", async (GetAllPostsHandler handler) =>
        {
            try
            {
                var response = await handler.HandleAsync();
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message );
            }
        });
    }
}