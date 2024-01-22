using System.IdentityModel.Tokens.Jwt;

namespace Shared;

public static class JwtUtil
{
    public static IDictionary<string, string> GetClaimsFromToken(string token)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler()
            .ReadJwtToken(token.Substring("Bearer ".Length));
        return jwtSecurityToken.Claims
            .ToDictionary(x => x.Type.ToString(), x => x.Value);
    }
}