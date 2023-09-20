using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
