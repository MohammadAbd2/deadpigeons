using System.Text;
using api;
using efscaffold;
using efscaffold.Models;
using api.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// =================== Configuration ===================
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var configuration = builder.Configuration;

// =================== AppOptions ===================
var appOptions = builder.Services.AddAppOptions(configuration);
if (string.IsNullOrEmpty(appOptions.JWTSecret))
    throw new InvalidOperationException("JWTSecret is not set. Configure it in appsettings.json or environment variables.");

builder.Services.AddSingleton(appOptions);

// =================== Services ===================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseNpgsql(
        "Host=ep-soft-resonance-ad7u4o8b-pooler.c-2.us-east-1.aws.neon.tech;" +
        "Database=deadpigeons;" +
        "Username=neondb_owner;" +
        "Password=npg_mgTa1eJtVC7o;" +
        "SSL Mode=Require;" +
        "Trust Server Certificate=true;" + 
        "Channel Binding=Require;",
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(5)
    );
});

// =================== Controllers & ProblemDetails ===================
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// =================== Swagger ===================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "Sample API with Swagger for local testing"
    });
});

// =================== CORS ===================
builder.Services.AddCors();

// =================== JWT Authentication ===================
var key = Encoding.ASCII.GetBytes(appOptions.JWTSecret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// =================== Middleware ===================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseCors(config => config
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .SetIsOriginAllowed(_ => true));

app.UseAuthentication();
app.UseAuthorization();

// =================== Map Controllers ===================
app.MapControllers();

app.Run();

