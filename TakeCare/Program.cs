using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Security;
using TakeCare.Application.Services;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;
using TakeCare.Security;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = JwtSettings.Load(builder.Configuration);
var connection = builder.Configuration.GetConnectionString("defaultConnection");
if (string.IsNullOrWhiteSpace(connection))
    throw new InvalidOperationException("Configure ConnectionStrings:defaultConnection outside source control.");
try
{
    var sql = new SqlConnectionStringBuilder(connection);
    if (!builder.Environment.IsDevelopment() &&
        (sql.TrustServerCertificate || sql.Encrypt == SqlConnectionEncryptOption.Optional || sql.PersistSecurityInfo))
        throw new ArgumentException();
}
catch (ArgumentException)
{
    throw new InvalidOperationException("Invalid database configuration. Production requires encrypted SQL with certificate validation.");
}
if (builder.Configuration.GetValue<bool>("Email:Enabled") &&
    new[] { "Email:Public", "Email:Secret", "Email:FromEmail" }.Any(k => string.IsNullOrWhiteSpace(builder.Configuration[k])))
    throw new InvalidOperationException("Email is enabled but its external configuration is incomplete.");

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
builder.Logging.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.AspNetCore.Mvc", LogLevel.Warning);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "__Host-takecare-csrf";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Path = "/";
});
builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 6 * 1024 * 1024);
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 6 * 1024 * 1024);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.SaveToken = false;
    options.IncludeErrorDetails = false;
    options.TokenValidationParameters = jwtSettings.ValidationParameters();
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (string.IsNullOrEmpty(context.Request.Headers.Authorization))
                context.Token = context.Request.Cookies[SessionCookies.Name];
            return Task.CompletedTask;
        },
        OnTokenValidated = async context =>
        {
            if (!int.TryParse(context.Principal?.FindFirstValue("sub"), NumberStyles.None,
                    CultureInfo.InvariantCulture, out var id))
            {
                context.Fail("Invalid session.");
                return;
            }
            var database = context.HttpContext.RequestServices.GetRequiredService<TakeCareDbContext>();
            var user = await database.UserTable!.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            if (user is null || user.Role != context.Principal?.FindFirstValue("role") ||
                user.Email != context.Principal?.FindFirstValue("email"))
                context.Fail("Invalid session.");
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("login", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown", _ => new FixedWindowRateLimiterOptions
        { PermitLimit = 10, Window = TimeSpan.FromMinutes(1), QueueLimit = 0, AutoReplenishment = true }));
});
builder.Services.AddDbContext<TakeCareDbContext>(options => options
    .EnableSensitiveDataLogging(false).EnableDetailedErrors(false).UseSqlServer(connection));
builder.Services.AddScoped(typeof(IGenericService<User>), typeof(GenericService<TakeCareDbContext, User>));
builder.Services.AddScoped(typeof(IGenericService<Patient>), typeof(GenericService<TakeCareDbContext, Patient>));
builder.Services.AddScoped(typeof(IGenericService<Doctor>), typeof(GenericService<TakeCareDbContext, Doctor>));
builder.Services.AddScoped(typeof(IGenericService<Address>), typeof(GenericService<TakeCareDbContext, Address>));
builder.Services.AddScoped(typeof(IGenericService<Receptionist>), typeof(GenericService<TakeCareDbContext, Receptionist>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IExaminationService, ExaminationService>();
builder.Services.AddHttpClient<IEmailService, EmailService>(client => client.Timeout = TimeSpan.FromSeconds(15));
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ResourceAccess>();

var app = builder.Build();
app.UseMiddleware<SafeErrorsMiddleware>();
if (!app.Environment.IsDevelopment()) app.UseHsts();
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    if (context.Request.Path.StartsWithSegments("/api")) context.Response.Headers.CacheControl = "no-store";
    await next();
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// The SPA and API share one HTTPS origin. Development uses the Vite proxy.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
app.Run();

public partial class Program { }
