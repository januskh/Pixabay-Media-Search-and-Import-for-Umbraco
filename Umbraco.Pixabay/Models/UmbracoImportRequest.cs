using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    public class UmbracoImportRequest
    {
        [JsonProperty("mediafolder")]
        public int MediaFolder { get; set; }

        [JsonProperty("idlist")]
        public List<int> IdList { get; set; }

        [JsonProperty("mediaType")]
        public int MediaType { get; set; }

    }
}
