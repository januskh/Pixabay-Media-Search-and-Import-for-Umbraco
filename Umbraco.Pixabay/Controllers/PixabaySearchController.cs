using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Umbraco.Pixabay.Enums;
using System.Web.Http;
using Umbraco.Pixabay.Search;
using Umbraco.Web.WebApi;
using Umbraco.Pixabay.Extensions;
using Umbraco.Pixabay.Models;

namespace Umbraco.Pixabay.Controllers
{
    public class PixabaySearchController : UmbracoAuthorizedApiController
    {
        internal Models.Settings settings = null;

        public PixabaySearchController()
        {
            PixabaySettingsController settings = new PixabaySettingsController();
            this.settings = settings.GetSettings();
        }

        //public static string ApiKey = "8070652-c65c9e408eabd62c9e8467431";


        [HttpPost]
        public Models.ImageSearchResult SearchImages([FromBody] SearchOptions options)
        {
            options.ApiKey = settings.ApiKey;
            return Images.SearchWithOptions(new ImageSearchOptions(options));
        }

        [HttpPost]
        public Models.VideoSearchResults SearchVideos([FromBody] SearchOptions options)
        {
            options.ApiKey = settings.ApiKey;
            return Videos.SearchWithOptions(new VideoSearchOptions(options));
        }

        [HttpGet]
        public AvailableOptions GetAvailableOptions()
        {
            AvailableOptions options = new AvailableOptions();

            foreach (EnumLanguage cat in Enum.GetValues(typeof(EnumLanguage)))
            {
                options.Language.Add(cat.GetDescription());
            }

            foreach (EnumResponseGroup cat in Enum.GetValues(typeof(EnumResponseGroup)))
            {
                options.ResponseGroup.Add(cat.GetDescription());
            }

            foreach (EnumImageType cat in Enum.GetValues(typeof(EnumImageType)))
            {
                options.ImageType.Add(cat.GetDescription());
            }

            foreach (EnumOrientation cat in Enum.GetValues(typeof(EnumOrientation)))
            {
                options.Orientation.Add(cat.GetDescription());
            }

            foreach (EnumCategory cat in Enum.GetValues(typeof(EnumCategory)))
            {
                options.Category.Add(cat.GetDescription());
            }

            foreach (EnumVideoType cat in Enum.GetValues(typeof(EnumVideoType)))
            {
                options.VideoType.Add(cat.GetDescription());
            }

            return options;
        }

    }
}
