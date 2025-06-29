public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Unidentified;
    public string Message { get; set; } = string.Empty;
}