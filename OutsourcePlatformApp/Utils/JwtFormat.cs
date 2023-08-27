using System.IdentityModel.Tokens.Jwt;

namespace OutsourcePlatformApp.Utils;

public static class JwtFormat
{
    public static string GetUsernameFromToken(string token)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token.Substring("Bearer ".Length));
        var username = jwtSecurityToken.Claims
            .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
            .Value;
        return username;
    }
}