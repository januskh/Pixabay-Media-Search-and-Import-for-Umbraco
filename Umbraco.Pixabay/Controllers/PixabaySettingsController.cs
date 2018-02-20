using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Umbraco.Web.WebApi;

namespace Umbraco.Pixabay.Controllers
{
    public class PixabaySettingsController: UmbracoAuthorizedApiController
    {

        private string filename = string.Empty;

        private Models.Settings settings = null;

        public PixabaySettingsController()
        {
            filename = HostingEnvironment.MapPath("/config/pixabay.config");
        }
       

        [HttpGet]
        public Models.Settings GetSettings()
        {
            Models.Settings settings = LoadSettingsFile();
            if (settings == null)
            {
                return new Models.Settings();
            }
            return settings;
        }

        [HttpPost]
        public bool SaveSettings([FromBody] Models.SettingsWrapper settings)
        {
            return SaveSettingsFile(settings.Settings);
        }

        private Models.Settings LoadSettingsFile()
        {
            if (System.IO.File.Exists(filename))
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var serializer = new XmlSerializer(typeof(Models.Settings));
                    return serializer.Deserialize(stream) as Models.Settings;
                }
            }

            return null;
        }

        private bool SaveSettingsFile(Models.Settings settings)
        {
            using (var writer = new System.IO.StreamWriter(filename))
            {
                var serializer = new XmlSerializer(typeof(Models.Settings));
                serializer.Serialize(writer, settings);
                writer.Flush();
                return true;
            }
        }

    }
}
