using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace store_api.Utils;

public class Utils
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public IConfiguration _configuration { get; }

    public Utils(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public bool ValidateToken(string token)
    {
        var key = _configuration["JwtSecretKey"];

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (!(validatedToken is JwtSecurityToken jwtToken) || 
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return false;

            var username = principal.Identity.Name;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static DateTime ConvertToDateTime(long timeStamp)
    {
        return Epoch.AddMilliseconds(timeStamp);
    }
}