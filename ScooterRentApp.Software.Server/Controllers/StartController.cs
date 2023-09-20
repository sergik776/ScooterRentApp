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
}
