public interface IUserStorage
{
    public Task<bool> UsernameExists(string username);
    public Task SaveUserAsync(User user);
    public Task<User?> GetUserByUsernameAsync(string username);
}