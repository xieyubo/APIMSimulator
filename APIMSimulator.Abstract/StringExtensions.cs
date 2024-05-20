using System.IdentityModel.Tokens.Jwt;

namespace APIMSimulator.Abstract;

public static class StringExtensions
{
    public static Jwt? AsJwt(this string str)
    {
        if (str.StartsWith("Bearer "))
        {
            str = str.Substring("Bearer ".Length);
        }
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.CanReadToken(str) ? new Jwt(jwtHandler.ReadJwtToken(str)) : null;
        return jwt;
    }
}
