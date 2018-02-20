using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    public class VideoSearchResults
    {
        [JsonProperty("totalHits")]
        public int TotalHits { get; set; }

        [JsonProperty("hits")]
        public List<VideoEntry> Videos { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
