using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TakeCare.Application.Security;

namespace TakeCare.Application.Services;

public sealed class JwtService(JwtSettings settings)
{
    public string Generate(int id, string role, string email)
    {
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(settings.Issuer, settings.Audience,
            [
                new Claim("sub", id.ToString(CultureInfo.InvariantCulture)),
                new Claim("role", role),
                new Claim("email", email),
                new Claim("jti", Guid.NewGuid().ToString("N")),
                new Claim("iat", new DateTimeOffset(now).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            ],
            notBefore: now, expires: now.AddMinutes(settings.AccessTokenMinutes),
            signingCredentials: new SigningCredentials(settings.SigningKey, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
