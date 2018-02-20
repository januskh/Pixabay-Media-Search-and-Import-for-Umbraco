using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    public class AvailableOptions
    {

        public List<string> Language { get; set; } = new List<string>();

        public List<string> ResponseGroup { get; set; } = new List<string>();

        public List<string> ImageType { get; set; } = new List<string>();

        public List<string> Orientation { get; set; } = new List<string>();

        public List<string> Category { get; set; } = new List<string>();

        public List<string> VideoType { get; set; } = new List<string>();

    }
}
