using ScooterRentApp.Software.Server;
using ScooterRentApp.Software.Server.Services;
using Keycloak.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Keycloak.Net.Models.Root;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.OpenApi.Models;
using ScooterRent.Software.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Если необходим доступ с учетными данными
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
IConfiguration configurationManager = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Укажите путь к корневой папке приложения
            .AddJsonFile("appsettings.json") // Укажите имя файла конфигурации
            .Build();

var configuration = configurationManager.GetSection("JwtSettings");
var certificateString = configuration["Certificate"]; // Загружаем сертификат из конфигурации
var certificate = new X509Certificate2(Encoding.UTF8.GetBytes(certificateString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Временно для упрощения настройки
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuration["Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new X509SecurityKey(certificate), // Используем сертификат для валидации
        ClockSkew = TimeSpan.Zero // Не учитываем разницу во времени (можно установить другое значение)
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // если запрос направлен хабу
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/ScooterHub"))
            {
                // получаем токен из строки запроса
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ScooterService",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddSingleton<ScooterListService>();
builder.Services.AddScoped<SendCommandService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddSingleton<ScooterEventHub>();
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
app.UseCors("AllowAnyOrigin");
app.MapGet("/hello", () => "HelloWorld");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ScooterEventHub>("/ScooterHub");
app.MapGrpcService<ScooterService>();
app.Run();