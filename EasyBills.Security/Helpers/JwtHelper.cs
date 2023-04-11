using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyBills.Security.Helpers;

/// <summary>
/// JWT common methods.
/// </summary>
public class JwtHelper
{
    /// <summary>
    /// Get secret key from the appsettings.
    /// </summary>
    /// <param name="configuration">Configuration object.</param>
    /// <returns>The secret key.</returns>
    /// <exception cref="Exception"></exception>
    public static string GetSecretKey(IConfiguration configuration)
    {
        string secretKey = configuration.GetSection("AppSettings").GetSection("JwtSecretKey").Value ?? throw new Exception("JwtSecretKey is not configured");

        return secretKey;
    }

    /// <summary>
    /// Create a JWT from user data.
    /// </summary>
    /// <param name="configuration">Configuration object.</param>
    /// <param name="id">User id.</param>
    /// <param name="fullName">Full name.</param>
    /// <param name="email">Email.</param>
    /// <param name="tokenLifeTime">Token life time.</param>
    /// <returns>Token.</returns>
    public static string CreateJWT(IConfiguration configuration, string id, string fullName, string email, int tokenLifeTime)
    {
        string secretKey = GetSecretKey(configuration);

        var identity = GenerateIdentity(id, fullName, email);
        var tokenHandler = new JwtSecurityTokenHandler() { TokenLifetimeInMinutes = tokenLifeTime };

        var token = tokenHandler.CreateJwtSecurityToken(
            subject: identity,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest)
            );

        var serializedToken = tokenHandler.WriteToken(token);

        return serializedToken;
    }

    /// <summary>
    /// Validate a JWT.
    /// </summary>
    /// <param name="configuration">Configuration object.</param>
    /// <param name="token">JSON Web token.</param>
    /// <returns>The user id if valid, null otherwise.</returns>
    public static string? ValidateJWT(IConfiguration configuration, string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        string secretKey = GetSecretKey(configuration);
        var simmetricSecurityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = simmetricSecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            };
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken.ValidTo < DateTime.UtcNow) return null;

            var jwtSecurityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            var userId = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid")?.Value;

            return userId;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Generate claims identity.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="fullName">Full name.</param>
    /// <param name="email">Email.</param>
    /// <returns>The identity object.</returns>
    public static ClaimsIdentity GenerateIdentity(string id, string fullName, string email)
    {
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
        identity.AddClaim(new Claim(ClaimTypes.Name, fullName));
        identity.AddClaim(new Claim(ClaimTypes.Email, email));

        return identity;
    }
}
