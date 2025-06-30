using Microsoft.AspNetCore.Identity;

public class RegisterUserHandler
{
    private readonly IUserStorage _userStorage;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(IUserStorage userStorage, PasswordHasher<User> passwordHasher, ILogger<RegisterUserHandler> logger)
    {
        _userStorage = userStorage;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    public async Task<RegisterUserResponse> HandleAsync(RegisterUserRequest request)
    {
        try
        {
            bool usernameExists = await _userStorage.UsernameExists(request.Username);
            if (usernameExists)
            {
                _logger.LogWarning("The username '{username}' already exists.", request.Username);
                throw new Exception("Username is taken, please try another one.");
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username.Trim().ToLower(),
                Email = request.Email,
                Role = request.Role,
                RegistrationDate = DateTime.UtcNow
            };
            user.HashedPassword = _passwordHasher.HashPassword(user, request.Password);
            _logger.LogInformation("Password hashed successfully.");
            await _userStorage.SaveUserAsync(user);
            _logger.LogInformation("User registered with username '{username}'.", user.Username);

            return new RegisterUserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while registering the user.");
            throw;
        }
    }
}