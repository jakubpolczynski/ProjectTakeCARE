using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Models;
using TakeCare.Application.Security;
using TakeCare.Application.Services;
using TakeCare.Database.Data;
using TakeCare.Security;

namespace TakeCare.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LoginController(TakeCareDbContext database, JwtService jwt, JwtSettings settings) : ControllerBase
{
    private static readonly string DummyHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N"), 12);

    [AllowAnonymous]
    [HttpGet("Csrf")]
    public IActionResult Csrf([FromServices] IAntiforgery antiforgery)
    {
        Response.Headers.CacheControl = "no-store";
        return Ok(new { token = antiforgery.GetAndStoreTokens(HttpContext).RequestToken });
    }

    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrEmpty(login.Password) ||
            Encoding.UTF8.GetByteCount(login.Password) > 72 || login.Email.Length > 254)
            return Unauthorized(new { error = "Invalid credentials." });
        var user = await database.UserTable!.AsNoTracking().SingleOrDefaultAsync(u => u.Email == login.Email);
        bool valid;
        try { valid = BCrypt.Net.BCrypt.Verify(login.Password, user?.Password ?? DummyHash); }
        catch (BCrypt.Net.SaltParseException) { valid = false; }
        if (!valid || user is null || user.Role is not ("Patient" or "Doctor" or "Receptionist" or "Admin"))
            return Unauthorized(new { error = "Invalid credentials." });
        Response.Cookies.Delete("jwt", SessionCookies.Options());
        Response.Cookies.Append(SessionCookies.Name, jwt.Generate(user.Id, user.Role, user.Email),
            SessionCookies.Options(DateTimeOffset.UtcNow.AddMinutes(settings.AccessTokenMinutes)));
        return Ok(new { message = "Success login" });
    }

    [Authorize]
    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser()
    {
        var id = ResourceAccess.UserId(User);
        var user = await database.UserTable!.AsNoTracking().Where(u => u.Id == id)
            .Select(u => new { u.Id, u.Email, u.Role }).SingleOrDefaultAsync();
        return user is null ? Unauthorized() : Ok(user);
    }

    [Authorize]
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(SessionCookies.Name, SessionCookies.Options());
        Response.Cookies.Delete("jwt", SessionCookies.Options());
        return Ok(new { message = "Success" });
    }
}
