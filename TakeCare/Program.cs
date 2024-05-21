using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Services;
using TakeCare.Database.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TakeCare.Database.Entity;

var builder = WebApplication.CreateBuilder(args);

//Jwt configuration
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtIssuer,
			ValidAudience = jwtIssuer,
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey!))
		};
	});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "ClientApp/dist";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:3000")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowAnyOrigin()
    );
});

var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

builder.Services.AddDbContext<TakeCareDbContext>(options =>
	options.UseSqlServer(connectionString),
	ServiceLifetime.Scoped);

builder.Services.AddScoped(typeof(IGenericService<User>), typeof(GenericService<TakeCareDbContext, User>));
builder.Services.AddScoped(typeof(IGenericService<Patient>), typeof(GenericService<TakeCareDbContext, Patient>));
builder.Services.AddScoped(typeof(IGenericService<Doctor>), typeof(GenericService<TakeCareDbContext, Doctor>));
builder.Services.AddScoped(typeof(IGenericService<Address>), typeof(GenericService<TakeCareDbContext, Address>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.Run();
