using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Service;

public class JwtTokenService
{
    private readonly IConfiguration configuration;


    public JwtTokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var secretKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.UserRoles.First().Name)
        };
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}