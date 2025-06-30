using Microsoft.AspNetCore.Identity;

public class LoginHandler
{
    private readonly IUserStorage _userStorage;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly TokenGenerator _tokenGenerator;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(IUserStorage userStorage, PasswordHasher<User> passwordHasher, TokenGenerator tokenGenerator, ILogger<LoginHandler> logger)
    {
        _userStorage = userStorage;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }
    public async Task<LoginResponse> HandleAsync(LoginRequest request)
    {
        try
        {

            var user = await _userStorage.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                _logger.LogWarning("No user with the username '{username}' is found.", request.Username);
                throw new Exception("Incorrect username, please try again.");
            }

            var isPasswordMatch = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);
            if (isPasswordMatch != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Incorrect password to the username '{username}'.", request.Username);
                throw new Exception("Incorrect password, please try again.");
            }

            var token = _tokenGenerator.GenerateToken(user);
            _logger.LogInformation("Successfully generated token to the user with username '{username}'.", user.Username);
            _logger.LogInformation("Login successful for user '{username}' with ID '{id}'.", user.Username, user.Id);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Message = "Login successful."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while loging in.");
            throw;
        }
    }
}