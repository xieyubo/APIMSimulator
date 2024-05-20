using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace APIMSimulator.Abstract;

public class Jwt
{
    public IReadOnlyDictionary<string, string[]> Claims { get; }

    public Jwt(JwtSecurityToken token)
    {
        var claims = new Dictionary<string, string[]>();
        var claimTypes = token.Claims.Select(c => c.Type);
        foreach (var claimType in claimTypes)
        {
            claims.Add(claimType, token.Claims.Where(c => c.Type == claimType).Select(c => c.Value).ToArray());
        }
        Claims = claims;
    }
}
