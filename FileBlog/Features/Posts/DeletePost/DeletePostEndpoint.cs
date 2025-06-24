public class DeletePostEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete("/posts/{slug}", async (DeletePostHandler handler, string slug) =>
        {
            try
            {
                var response = await handler.HandleAsync(slug);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { Message = ex.Message });
            }
        });
    }
}