using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TakeCare.Application.Security;
using TakeCare.Application.Services;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;
using TakeCare.Security;
using Xunit;

namespace TakeCare.SecurityTests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public byte[] Key { get; } = RandomNumberGenerator.GetBytes(64);
    private readonly string databaseName = Guid.NewGuid().ToString("N");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("Jwt:SigningKeyBase64", Convert.ToBase64String(Key));
        builder.UseSetting("Jwt:Issuer", "test-issuer");
        builder.UseSetting("Jwt:Audience", "test-audience");
        builder.UseSetting("ConnectionStrings:defaultConnection", "Server=localhost;Database=Unused;Integrated Security=true;Encrypt=true");
        builder.UseSetting("Email:Enabled", "false");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<TakeCareDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<TakeCareDbContext>>();
            services.RemoveAll<TakeCareDbContext>();
            services.AddDbContext<TakeCareDbContext>(options => options.UseInMemoryDatabase(databaseName));
        });
    }

    public HttpClient Client() => CreateClient(new WebApplicationFactoryClientOptions
        { BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false, HandleCookies = true });

    public async Task Seed()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
        await db.Database.EnsureDeletedAsync();
        var patient1 = new Patient { Id = 1, UserId = 1, Email = "one@example.invalid",
            User = new User { Id = 1, Email = "one@example.invalid", Role = "Patient", Password = BCrypt.Net.BCrypt.HashPassword("Test-password-123!", 4) },
            Address = new Address { Id = 1 } };
        var patient2 = new Patient { Id = 2, UserId = 2, Email = "two@example.invalid",
            User = new User { Id = 2, Email = "two@example.invalid", Role = "Patient", Password = BCrypt.Net.BCrypt.HashPassword("Test-password-123!", 4) },
            Address = new Address { Id = 2 } };
        var doctor = new Doctor { Id = 1, UserId = 3, Email = "doctor@example.invalid",
            User = new User { Id = 3, Email = "doctor@example.invalid", Role = "Doctor", Password = BCrypt.Net.BCrypt.HashPassword("Test-password-123!", 4) } };
        db.PatientTable!.AddRange(patient1, patient2);
        db.DoctorTable!.Add(doctor);
        db.UserTable!.Add(new User { Id = 4, Email = "reception@example.invalid", Role = "Receptionist" });
        db.VisitTable!.Add(new Visit { Id = 1, Patient = patient1, Doctor = doctor, Date = new DateTime(2026, 11, 1, 12, 0, 0) });
        db.ExaminationTable!.Add(new Examination { Id = 1, Patient = patient1, Doctor = doctor,
            VisitId = 1, Date = DateTime.UtcNow, Name = "private-result", Description = "medical-data", Result = "private-data", Type = "test" });
        await db.SaveChangesAsync();
    }

    public string Token(int id, string role, string email)
    {
        using var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<JwtService>().Generate(id, role, email);
    }

    public static async Task Csrf(HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("X-CSRF-TOKEN");
        var data = await client.GetFromJsonAsync<JsonElement>("/api/Login/Csrf");
        client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", data.GetProperty("token").GetString());
    }
}

public sealed class SecurityTests
{
    [Theory]
    [InlineData("/api/Visit/GetVisit?id=1")]
    [InlineData("/api/Visit/GetPatientVisits?patientEmail=one@example.invalid")]
    [InlineData("/api/Examination/GetPatientExaminations?patientEmail=one@example.invalid")]
    [InlineData("/api/Patient/GetDoctorPatients?email=doctor@example.invalid")]
    [InlineData("/api/User?id=1")]
    [InlineData("/api/Login/GetUser")]
    public async Task Anonymous_medical_and_account_requests_are_rejected(string route)
    {
        await using var factory = new ApiFactory();
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync(route)).StatusCode);
    }

    [Fact]
    public async Task Patient_cannot_read_another_patients_visit_or_results()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(2, "Patient", "two@example.invalid"));
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/Visit/GetVisit?id=1")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/Examination/GetPatientExaminations?patientEmail=one@example.invalid")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/User?id=1")).StatusCode);
    }

    [Fact]
    public async Task Owner_can_read_their_visit_and_results()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Patient", "one@example.invalid"));
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/Visit/GetVisit?id=1")).StatusCode);
        var response = await client.GetAsync("/api/Examination/GetPatientExaminations?patientEmail=one@example.invalid");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("private-result", await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData("AddDoctor")]
    [InlineData("AddReceptionist")]
    public async Task Privileged_registration_requires_admin(string action)
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/User/" + action, new { })).StatusCode);
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Patient", "one@example.invalid"));
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync("/api/User/" + action, new { })).StatusCode);
    }

    [Fact]
    public async Task Cookie_login_csrf_and_cookie_attributes_work()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        var body = new { Email = "one@example.invalid", Password = "Test-password-123!" };
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/api/Login/Login", body)).StatusCode);
        await ApiFactory.Csrf(client);
        var response = await client.PostAsJsonAsync("/api/Login/Login", body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var cookie = response.Headers.GetValues("Set-Cookie").Single(v => v.StartsWith(SessionCookies.Name + "="));
        Assert.Contains("secure", cookie.ToLowerInvariant());
        Assert.Contains("httponly", cookie.ToLowerInvariant());
        Assert.Contains("samesite=strict", cookie.ToLowerInvariant());
        Assert.DoesNotContain("domain=", cookie.ToLowerInvariant());
        var profile = await client.GetAsync("/api/Login/GetUser");
        Assert.Equal(HttpStatusCode.OK, profile.StatusCode);
        Assert.DoesNotContain("password", (await profile.Content.ReadAsStringAsync()).ToLowerInvariant());
        // An anonymous CSRF token is deliberately rejected after the identity changes.
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsync("/api/Login/Logout", null)).StatusCode);
        await ApiFactory.Csrf(client);
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsync("/api/Login/Logout", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Login/GetUser")).StatusCode);
    }

    [Theory]
    [InlineData("issuer")]
    [InlineData("audience")]
    [InlineData("expired")]
    [InlineData("signature")]
    [InlineData("algorithm")]
    public async Task Invalid_tokens_are_rejected(string mutation)
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        var now = DateTime.UtcNow;
        var key = mutation == "signature" ? RandomNumberGenerator.GetBytes(32) : factory.Key;
        var algorithm = mutation == "algorithm" ? SecurityAlgorithms.HmacSha384 : SecurityAlgorithms.HmacSha256;
        var token = new JwtSecurityToken(
            mutation == "issuer" ? "wrong" : "test-issuer",
            mutation == "audience" ? "wrong" : "test-audience",
            [new Claim("sub", "1"), new Claim("role", "Patient"), new Claim("email", "one@example.invalid")],
            now.AddMinutes(-30), mutation == "expired" ? now.AddMinutes(-10) : now.AddMinutes(5),
            new SigningCredentials(new SymmetricSecurityKey(key), algorithm));
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", new JwtSecurityTokenHandler().WriteToken(token));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Login/GetUser")).StatusCode);
    }

    [Fact]
    public async Task Deleted_accounts_and_changed_roles_invalidate_sessions()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Doctor", "one@example.invalid"));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Login/GetUser")).StatusCode);
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(999, "Patient", "missing@example.invalid"));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Login/GetUser")).StatusCode);
    }

    [Fact]
    public async Task Tokens_have_short_lifetime_and_no_encrypted_issuer()
    {
        await using var factory = new ApiFactory();
        var token = new JwtSecurityTokenHandler().ReadJwtToken(factory.Token(7, "Patient", "test@example.invalid"));
        Assert.Equal("test-issuer", token.Issuer);
        Assert.Equal("7", token.Subject);
        Assert.Single(token.Claims, c => c.Type == "jti");
        Assert.InRange((token.ValidTo - token.ValidFrom).TotalMinutes, 1, 15);
    }

    [Fact]
    public async Task Resource_checks_and_visit_mapping_execute_on_the_database_provider()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var scope = factory.Services.CreateScope();
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "1"), new Claim("role", "Patient")], "test", "sub", "role"));
        Assert.True(await scope.ServiceProvider.GetRequiredService<ResourceAccess>().CanReadVisit(user, 1));
        Assert.Equal(1, (await scope.ServiceProvider.GetRequiredService<TakeCare.Application.Interfaces.IVisitService>().GetVisit(1)).Id);
    }

    [Fact]
    public async Task Registration_assigns_patient_role_and_never_echoes_password()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        await ApiFactory.Csrf(client);
        var body = new { Email = "new@example.invalid", Password = "Test-password-123!", Role = "Admin",
            FirstName = "Test", LastName = "Patient", Pesel = "00000000000", Phone = "123456789",
            City = "Test", Street = "Test", PostalCode = "00-001" };
        var response = await client.PostAsJsonAsync("/api/User/AddPatient", body);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.DoesNotContain("password", (await response.Content.ReadAsStringAsync()).ToLowerInvariant());
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
        var user = await db.UserTable!.SingleAsync(u => u.Email == body.Email);
        Assert.Equal("Patient", user.Role);
        Assert.NotEqual(body.Password, user.Password);
        Assert.True(BCrypt.Net.BCrypt.Verify(body.Password, user.Password));
    }

    [Theory]
    [InlineData("two@example.invalid", false)]
    [InlineData("one@example.invalid", true)]
    public async Task Booking_cannot_impersonate_another_patient_or_mark_a_visit_executed(string email, bool executed)
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Patient", "one@example.invalid"));
        await ApiFactory.Csrf(client);
        var response = await client.PostAsJsonAsync("/api/Visit/BookVisit", new
        { DoctorEmail = "doctor@example.invalid", PatientEmail = email, Slot = DateTime.UtcNow.AddDays(1), Reason = "test", IsVisitExecuted = executed });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        using var scope = factory.Services.CreateScope();
        Assert.Equal(1, await scope.ServiceProvider.GetRequiredService<TakeCareDbContext>().VisitTable!.CountAsync());
    }

    [Fact]
    public async Task Profile_update_cannot_change_another_account_password_or_role()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Patient", "one@example.invalid"));
        await ApiFactory.Csrf(client);
        var body = new { PatientId = 2, Email = "two@example.invalid", Password = "Attempted-change-123!", Role = "Admin",
            FirstName = "Changed", LastName = "Patient", Pesel = "11111111111", Phone = "123456789",
            City = "Test", Street = "Test", PostalCode = "00-001" };
        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsJsonAsync("/api/User/UpdatePatient", body)).StatusCode);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
        var ownUser = await db.UserTable!.SingleAsync(u => u.Id == 1);
        Assert.Equal("Patient", ownUser.Role);
        Assert.Equal("one@example.invalid", ownUser.Email);
        Assert.True(BCrypt.Net.BCrypt.Verify("Test-password-123!", ownUser.Password));
        Assert.Equal("Changed", (await db.PatientTable!.SingleAsync(p => p.Id == 1)).FirstName);
        Assert.NotEqual("Changed", (await db.PatientTable!.SingleAsync(p => p.Id == 2)).FirstName);
    }

    [Fact]
    public async Task Login_rate_limit_rejects_the_eleventh_attempt()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using var client = factory.Client();
        await ApiFactory.Csrf(client);
        for (var i = 0; i < 10; i++)
            Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/Login/Login",
                new { Email = "one@example.invalid", Password = "wrong" })).StatusCode);
        Assert.Equal(HttpStatusCode.TooManyRequests, (await client.PostAsJsonAsync("/api/Login/Login",
            new { Email = "one@example.invalid", Password = "wrong" })).StatusCode);
    }

    [Fact]
    public async Task Exceptions_do_not_echo_sensitive_text_in_logs_or_responses()
    {
        await using var factory = new ApiFactory();
        var messages = new List<string>();
        var logger = new RecordingLogger(messages);
        var middleware = new SafeErrorsMiddleware(_ => throw new InvalidOperationException("SENSITIVE_TEST_SENTINEL"), logger);
        var context = new DefaultHttpContext { RequestServices = factory.Services };
        context.Response.Body = new MemoryStream();
        await middleware.InvokeAsync(context);
        context.Response.Body.Position = 0;
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Equal(500, context.Response.StatusCode);
        Assert.DoesNotContain("SENSITIVE_TEST_SENTINEL", response);
        Assert.All(messages, m => Assert.DoesNotContain("SENSITIVE_TEST_SENTINEL", m));
        Assert.Contains(messages, m => m.Contains("InvalidOperationException"));
    }

    private sealed class RecordingLogger(List<string> messages) : ILogger<SafeErrorsMiddleware>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel level) => true;
        public void Log<TState>(LogLevel level, EventId id, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) => messages.Add(formatter(state, exception));
    }

    [Fact]
    public async Task Duplicate_patient_email_does_not_expose_another_accounts_medical_data()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
            var other = await db.PatientTable!.SingleAsync(p => p.Id == 2);
            other.Email = "one@example.invalid";
            var doctor = await db.DoctorTable!.SingleAsync(d => d.Id == 1);
            var visit = new Visit { Id = 2, Patient = other, Doctor = doctor, Date = DateTime.UtcNow, IsVisitExecuted = true };
            db.VisitTable!.Add(visit);
            db.ExaminationTable!.Add(new Examination { Id = 2, Patient = other, Doctor = doctor, Visit = visit,
                Date = DateTime.UtcNow, Name = "another-patient-secret", Description = "private", Result = "private", Type = "test" });
            await db.SaveChangesAsync();
        }
        using var client = factory.Client();
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Patient", "one@example.invalid"));
        foreach (var route in new[] { "/api/Examination/GetPatientExaminations", "/api/Visit/GetPatientVisits", "/api/Visit/GetPatientExecutedVisits" })
        {
            var response = await client.GetAsync(route);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.DoesNotContain("another-patient-secret", await response.Content.ReadAsStringAsync());
            if (route.EndsWith("GetPatientExecutedVisits")) Assert.Equal("[]", await response.Content.ReadAsStringAsync());
        }
    }

    [Fact]
    public void Missing_or_weak_signing_keys_stop_startup_without_echo()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        { ["Jwt:Issuer"] = "test", ["Jwt:Audience"] = "test", ["Jwt:SigningKeyBase64"] = "placeholder" }).Build();
        var exception = Assert.Throws<InvalidOperationException>(() => JwtSettings.Load(config));
        Assert.DoesNotContain("placeholder", exception.Message);
    }
}
