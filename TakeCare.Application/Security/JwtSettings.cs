using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TakeCare.Application.Security;

public sealed class JwtSettings
{
    public string Issuer { get; init; } = "";
    public string Audience { get; init; } = "";
    public int AccessTokenMinutes { get; init; } = 15;
    public required SymmetricSecurityKey SigningKey { get; init; }

    public static JwtSettings Load(IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Configure Jwt:Issuer and Jwt:Audience.");
        byte[] key;
        try { key = Convert.FromBase64String(configuration["Jwt:SigningKeyBase64"] ?? ""); }
        catch (FormatException) { throw new InvalidOperationException("Configure Jwt:SigningKeyBase64 using a new random key."); }
        if (key.Length < 32 || key.Distinct().Count() < 16)
            throw new InvalidOperationException("Jwt:SigningKeyBase64 must encode at least 32 random bytes.");
        var minutes = configuration.GetValue<int?>("Jwt:AccessTokenMinutes") ?? 15;
        if (minutes is < 1 or > 15)
            throw new InvalidOperationException("Jwt:AccessTokenMinutes must be between 1 and 15.");
        return new JwtSettings { Issuer = issuer, Audience = audience, AccessTokenMinutes = minutes,
            SigningKey = new SymmetricSecurityKey(key) };
    }

    public TokenValidationParameters ValidationParameters() => new()
    {
        ValidateIssuerSigningKey = true, IssuerSigningKey = SigningKey,
        ValidateIssuer = true, ValidIssuer = Issuer,
        ValidateAudience = true, ValidAudience = Audience,
        ValidateLifetime = true, RequireExpirationTime = true, RequireSignedTokens = true,
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256], ClockSkew = TimeSpan.FromSeconds(30),
        NameClaimType = "sub", RoleClaimType = "role"
    };
}
