/*
 * File: Program.cs
 * Author: Mohamed Farthas
 * Description: Application entry point and startup configuration for the
 *              Smart Solar Microgrid Web Service (FAT-service API). Registers
 *              the MongoDB connection, CORS, and controller routing.
 */

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebService.Models;
using WebService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Bind the "MongoDbSettings" section (connection string + database name) so it can
// be injected anywhere via IOptions<MongoDbSettings>, and register the shared
// MongoDbService as a singleton so every controller/repository reuses one connection.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddScoped<DatabaseSeeder>();

// Bind JWT settings and register the token service used by AuthController to
// issue tokens on login.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtTokenService>();

// Any endpoint marked [Authorize] will require a valid JWT (issued by /api/auth/login)
// in the "Authorization: Bearer <token>" header. This is what lets a client stay
// logged in across refreshes/restarts by resending its saved token.
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });
builder.Services.AddAuthorization();

// Web App (browser) and Android app both call this API from a different origin/host,
// so CORS must be open for development. Tighten this before production deployment on IIS.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Insert the one fixed Backoffice admin account if the Users collection is
// still empty. Safe to run on every startup — see DatabaseSeeder for why.
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowClients");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
