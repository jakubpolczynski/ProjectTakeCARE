using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Services;
using TakeCare.Database.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "ClientApp/dist";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", policyBuilder =>
        policyBuilder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Location")
                        .AllowAnyOrigin()
    );
});

var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

builder.Services.AddDbContext<TakeCareDBContext>(options =>
	options.UseSqlServer(connectionString),
	ServiceLifetime.Scoped);

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
