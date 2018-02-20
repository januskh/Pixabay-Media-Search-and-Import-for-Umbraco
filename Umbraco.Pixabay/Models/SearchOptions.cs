using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    [JsonObject]
    public class SearchOptions
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("searchTerm")]
        public string SearchTerm { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("responseGroup")]
        public string ResponseGroup { get; set; }

        [JsonProperty("imageType")]
        public string ImageType { get; set; }

        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("videoType")]
        public string VideoType { get; set; }

        [JsonProperty("editorsChoice")]
        public bool EditorsChoice { get; set; }

        [JsonProperty("safeSearch")]
        public bool SafeSearch { get; set; }

        [JsonProperty("minWidth")]
        public int MinWidth { get; set; }

        [JsonProperty("minHeight")]
        public int MinHeight { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("perPage")]
        public int PerPage { get; set; }

    }
}
