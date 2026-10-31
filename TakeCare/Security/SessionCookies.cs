namespace TakeCare.Security;

public static class SessionCookies
{
    public const string Name = "__Host-takecare-session";
    public static CookieOptions Options(DateTimeOffset? expires = null) => new()
    {
        HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict,
        Path = "/", IsEssential = true, Expires = expires
    };
}
