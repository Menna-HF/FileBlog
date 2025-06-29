using Microsoft.AspNetCore.Identity;

public class LoginHandler
{
    private readonly IUserStorage _userStorage;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly TokenGenerator _tokenGenerator;

    public LoginHandler(IUserStorage userStorage, PasswordHasher<User> passwordHasher, TokenGenerator tokenGenerator)
    {
        _userStorage = userStorage;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<LoginResponse> HandleAsync(LoginRequest request)
    {
        var user = await _userStorage.GetUserByUsernameAsync(request.Username);
        if (user == null)
            throw new Exception("Incorrect username, please try again.");

        var isPasswordMatch = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);
        if (isPasswordMatch != PasswordVerificationResult.Success)
            throw new Exception("Incorrect password, please try again.");

        var token = _tokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Message = "Login successful."
        }; 
    }
}