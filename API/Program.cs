using DAL;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Entities;
using Service.IService;
using Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Service.Service;
using Microsoft.Extensions.Logging;
using Service.Common.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var build = new ConfigurationBuilder();
build.AddJsonFile("appsettings.json");
var configuration = build.Build();

Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

builder.Services.AddSingleton(Log.Logger);
builder.Services.Configure<DbContextSettings>(configuration);
builder.Services.AddService(configuration);
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:4200", "https://another-allowed-origin.com")); // Add allowed origins here
});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
                          options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add DbContext
builder.Services.AddDbContextFactory<ErpDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // Enable detailed logging
           .EnableDetailedErrors()       // Enable detailed errors
           .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()))); // Use Serilog for logging

// Add Authentication
var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Injection de dépendances pour les services applicatifs
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPayeService, PayeService>();
builder.Services.AddScoped<IAssiduiteService, AssiduiteService>();
builder.Services.AddScoped<ICongeService, CongeService>();
builder.Services.AddScoped<IDepartementService, DepartementService>();
builder.Services.AddScoped<IPersonnelService, PersonnelService>();
var app = builder.Build();

app.UseRouting();
app.UseCors("CORSPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
