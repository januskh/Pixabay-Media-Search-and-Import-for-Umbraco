using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
   public class UmbracoImportResponse
    {
        [JsonProperty("successList")]
        public List<ImportEntry> SuccessList { get; set; } = new List<ImportEntry>();

        [JsonProperty("failureList")]
        public List<ImportEntry> FailureList { get; set; } = new List<ImportEntry>();

    }
}
