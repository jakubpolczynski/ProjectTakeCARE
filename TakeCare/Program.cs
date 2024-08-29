using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Services;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "ClientApp/dist";
});

builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString("defaultConnection");


builder.Services.AddDbContext<TakeCareDbContext>(options => 
	{
		options.EnableSensitiveDataLogging(true);
		options.UseSqlServer(connectionString);
	},
	ServiceLifetime.Scoped);

builder.Services.AddScoped(typeof(IGenericService<User>), typeof(GenericService<TakeCareDbContext, User>));
builder.Services.AddScoped(typeof(IGenericService<Patient>), typeof(GenericService<TakeCareDbContext, Patient>));
builder.Services.AddScoped(typeof(IGenericService<Doctor>), typeof(GenericService<TakeCareDbContext, Doctor>));
builder.Services.AddScoped(typeof(IGenericService<Address>), typeof(GenericService<TakeCareDbContext, Address>));
builder.Services.AddScoped(typeof(IGenericService<Receptionist>), typeof(GenericService<TakeCareDbContext, Receptionist>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IExaminationService, ExaminationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseCors(options => options
	.WithOrigins("https://localhost:3000")
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
);

app.Run();
