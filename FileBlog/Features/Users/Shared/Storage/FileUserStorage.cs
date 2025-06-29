using System.Text.Json;

public class FileUserStorage : IUserStorage
{
    private readonly string _userStorageFolder = Path.Combine("content", "users");
    public FileUserStorage()
    {
        Directory.CreateDirectory(_userStorageFolder);
    }
    private string[] GetMetaFiles()
    {
        return Directory.GetFiles(_userStorageFolder, "profile.json", SearchOption.AllDirectories);
    }
    public async Task<bool> UsernameExists(string username)
    {
        var metaFiles = GetMetaFiles();
        foreach (var file in metaFiles)
        {
            var json = await File.ReadAllTextAsync(file);
            var user = JsonSerializer.Deserialize<User>(json);
            if (!string.IsNullOrEmpty(user?.Username) && user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
    public async Task SaveUserAsync(User user)
    {
        var folderPath = Path.Combine(_userStorageFolder, user.Username.Trim().ToLower());
        Directory.CreateDirectory(folderPath);

        var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(folderPath, "profile.json");
        await File.WriteAllTextAsync(filePath, json);
    }
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var metaFiles = GetMetaFiles();
        foreach (var file in metaFiles)
        {
            var json = await File.ReadAllTextAsync(file);
            var user = JsonSerializer.Deserialize<User>(json);
            if (!string.IsNullOrEmpty(user?.Username) && user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                return user;
        }
        return null;
    }
}