using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.Root;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using ScooterRentApp.Software.Server;
using ScooterRentApp.Software.Server.Controllers;
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
            foreach(var r in Context.GetHttpContext().User.RolesScooterAuth())
            {
                Groups.AddToGroupAsync(Context.ConnectionId, r).Wait();
            }
            return base.OnConnectedAsync();
        }

        public async Task SendEventProperty(string model)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("ScooterPropercyChanged", "1122334455");
            }
        }
    }
}
