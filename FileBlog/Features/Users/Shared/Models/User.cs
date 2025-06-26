public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Unidentified;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
public enum UserRole
{
    Unidentified,
    Admin,
    Author,
    Editor
}