using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenGenerator
{
    private readonly string _secretKey;

    public TokenGenerator(string secretKey)
    {
        _secretKey = secretKey;
    }
    
    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            issuer: "FileBlog",
            audience: "http://localhost:5054/",
            claims: new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim (ClaimTypes.Name, user.Username),
                new Claim (ClaimTypes.Role, user.Role.ToString()),
            },
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}