using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace ScooterRentApp.Software.Server.Controllers
{
    public class StartController : ControllerBase
    {
        public IEnumerable<string> RolesScooterAuth { get { return GetRolesScooterAuth(JSON()); } }

        private JObject JSON()
        {
            var Claims = HttpContext.User.Claims;
            var resource_access = Claims.FirstOrDefault(x => x.Type == "resource_access");
            return JObject.Parse(resource_access.Value);
        }

        private IEnumerable<string> GetRolesScooterAuth(JObject json)
        {
            return json["ScooterAuth"]["roles"].ToObject<List<string>>();
        }
    }

    public static class Extentions
    {
        public static IEnumerable<string> RolesScooterAuth(this ClaimsPrincipal user)
        {
            var Claims = user.Claims;
            var resource_access = Claims.FirstOrDefault(x => x.Type == "resource_access");
            var json = JObject.Parse(resource_access.Value);
            return json["ScooterAuth"]["roles"].ToObject<List<string>>();
        }
    }
}
