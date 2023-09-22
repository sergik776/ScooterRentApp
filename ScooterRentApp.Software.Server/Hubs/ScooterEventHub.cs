using Keycloak.Net.Models.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using ScooterRentApp.Software.Server;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ScooterRent.Software.Server.Hubs
{
    [Authorize]
    public class ScooterEventHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            //if (Context.GetHttpContext() == null)
            //{
            //    Console.WriteLine("Поключение хаба, контекст пустой");
            //    return base.OnConnectedAsync();
            //}
            //if (IsValidToken(Context.GetHttpContext()))
            //{
            //    Console.WriteLine("подключение хаба - валидирован");
            //    return base.OnConnectedAsync();
            //}
            //else
            //{
            //    Console.WriteLine("Подключение хаба - не валидирован - отключен");
            //    Context.Abort();
            //    return base.OnConnectedAsync();
            //}
            return base.OnConnectedAsync();
        }

        public async Task SendEventProperty(ScooterPropertyUpdateRequest model)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("ScooterPropercyChanged", "1122334455");
            }
        }

        public async Task SendEventConnect(ScooterRequest model)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("ScooterConnected", "1122334455");
            }
        }

        private bool IsValidToken(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                IConfiguration configurationManager = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Укажите путь к корневой папке приложения
                .AddJsonFile("appsettings.json") // Укажите имя файла конфигурации
                .Build();
                var configuration = configurationManager.GetSection("JwtSettings");
                byte[] certificateBytes = Convert.FromBase64String(configuration["Certificate"]);
                X509Certificate2 certificate = new X509Certificate2(certificateBytes);
                var validationParameters = new TokenValidationParameters
                {
                    // Укажите правильные параметры валидации, включая ValidIssuer и ValidAudience
                    ValidIssuer = "http://localhost:8080/realms/TaogarSmartCloud",
                    ValidAudience = "account",
                    IssuerSigningKey = new RsaSecurityKey(certificate.GetRSAPublicKey()),
                };
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
