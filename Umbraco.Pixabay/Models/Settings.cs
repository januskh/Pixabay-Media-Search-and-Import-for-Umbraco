using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    [JsonObject]
    public class Settings
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

    }
}
