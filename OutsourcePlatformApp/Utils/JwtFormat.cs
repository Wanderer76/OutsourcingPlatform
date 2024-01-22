using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OutsourcePlatformApp.Utils;

public static class JwtFormat
{
    public static string GetUsernameFromToken(string token)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token.Substring("Bearer ".Length));
        var username = jwtSecurityToken.Claims
            .First(claim => claim.Type == ClaimTypes.Name)
            .Value;
        return username;
    }
}