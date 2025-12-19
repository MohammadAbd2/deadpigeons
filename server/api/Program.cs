using System.Text;
using api;
using api.services;
using efscaffold;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// =================== Configuration ===================
// builder.Configuration تلقائيًا يقرأ:
// - appsettings.json
// - appsettings.{Environment}.json
// - Environment Variables
var configuration = builder.Configuration;

// =================== AppOptions ===================
var appOptions = builder.Services.AddAppOptions(configuration);

if (string.IsNullOrWhiteSpace(appOptions.JWTSecret))
{
    throw new InvalidOperationException(
        "JWTSecret is not set. Configure it in appsettings.json or environment variables."
    );
}

if (string.IsNullOrWhiteSpace(appOptions.DbConnectionString))
{
    throw new InvalidOperationException(
        "DbConnectionString is not set. Configure it in appsettings.json or environment variables."
    );
}

builder.Services.AddSingleton(appOptions);

// =================== Database ===================
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseNpgsql(
        appOptions.DbConnectionString,
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(5);
        }
    );
});

// =================== Services ===================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// =================== Controllers & ProblemDetails ===================
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// =================== Swagger ===================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DeadPigeon API",
        Version = "v1",
        Description = "API for DeadPigeon project"
    });

    // JWT support in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =================== CORS ===================
builder.Services.AddCors();

// =================== JWT Authentication ===================
var key = Encoding.UTF8.GetBytes(appOptions.JWTSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeadPigeon API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
