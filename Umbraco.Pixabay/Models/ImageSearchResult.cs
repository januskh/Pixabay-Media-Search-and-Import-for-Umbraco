using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    [JsonObject]
    public class ImageSearchResult
    {

        [JsonProperty("totalHits")]
        public int TotalHits { get; set; }

        [JsonProperty("hits")]
        public List<ImageEntry> Images { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

    }
}
