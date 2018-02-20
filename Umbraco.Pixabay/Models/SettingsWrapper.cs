using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    public class SettingsWrapper
    {
        [JsonProperty("settings")]
        public Models.Settings Settings { get; set; }

    }
}
