public class LoginEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/login", async (LoginValidator validator, LoginHandler handler, LoginRequest request) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(x => new { x.PropertyName, x.ErrorMessage });
                return Results.BadRequest(errors);
            }

            var response = await handler.HandleAsync(request);
            return Results.Ok(response);
        });
    }
}