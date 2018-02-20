using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    public class ImportEntry
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("importTime")]
        public long ImportTime { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonProperty("error")]
        public string Error { get; set; }

    }
}
