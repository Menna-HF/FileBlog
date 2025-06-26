using Microsoft.AspNetCore.Identity;

public class RegisterUserHandler
{
    private readonly IUserStorage _userStorage;
    private readonly PasswordHasher<User> _passwordHasher;
    public RegisterUserHandler(IUserStorage userStorage, PasswordHasher<User> passwordHasher)
    {
        _userStorage = userStorage;
        _passwordHasher = passwordHasher;
    }
    public async Task<RegisterUserResponse> HandleAsync(RegisterUserRequest request)
    {
        bool usernameExists = await _userStorage.UsernameExists(request.Username);
        if (usernameExists)
            throw new Exception("Username is taken, please try another one.");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username.Trim(),
            Email = request.Email,
            Role = request.Role,
            RegistrationDate = DateTime.UtcNow
        };
        user.HashedPassword = _passwordHasher.HashPassword(user, request.Password);
        await _userStorage.SaveUserAsync(user);

        return new RegisterUserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }
}