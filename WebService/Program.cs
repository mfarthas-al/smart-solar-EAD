/*
 * File: Program.cs
 * Author: Mohamed Farthas
 * Description: Application entry point and startup configuration for the
 *              Smart Solar Microgrid Web Service (FAT-service API). Registers
 *              the MongoDB connection, CORS, and controller routing.
 */

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowClients");

app.UseAuthorization();

app.MapControllers();

app.Run();
