public class UpdatePostEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPut("/posts/{slug}", async (UpdatePostHandler handler, UpdatePostRequest request, UpdatePostValidator validator, string slug) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(x => new { x.PropertyName, x.ErrorMessage });
                return Results.BadRequest(errors);
            }
            try
            {
                var response = await handler.HandleAsync(request, slug);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new {Message = ex.Message});
            }
        }).RequireAuthorization("AllowedToUpdatePosts");
    }   
}