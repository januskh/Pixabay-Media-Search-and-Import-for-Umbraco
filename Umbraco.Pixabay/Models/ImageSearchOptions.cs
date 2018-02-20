using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Umbraco.Pixabay.Enums;
using Umbraco.Pixabay.Extensions;

namespace Umbraco.Pixabay.Models
{
    public class ImageSearchOptions
    {

        public ImageSearchOptions()
        {

        }

        public ImageSearchOptions(SearchOptions options)
        {
            SearchTerm = options.SearchTerm;
            ApiKey = options.ApiKey;
            if (!string.IsNullOrEmpty(options.Language)) Language = (EnumLanguage) options.Language.ToEnum<EnumLanguage>(true);
            if (!string.IsNullOrEmpty(options.ResponseGroup)) ResponseGroup = (EnumResponseGroup)options.ResponseGroup.ToEnum<EnumResponseGroup>(true);
            if (!string.IsNullOrEmpty(options.ImageType)) ImageType = (EnumImageType)options.ImageType.ToEnum<EnumImageType>(true);
            if (!string.IsNullOrEmpty(options.Orientation)) Orientation = (EnumOrientation)options.Orientation.ToEnum<EnumOrientation>(true);
            if (!string.IsNullOrEmpty(options.Category)) Category = (EnumCategory)options.Category.ToEnum<EnumCategory>(true);
            MinWidth = options.MinWidth;
            MinHeight = options.MinHeight;
            EditorsChoice = options.EditorsChoice;
            SafeSearch = options.SafeSearch;
            Page = options.Page;
            PerPage = options.PerPage;
        }


        /// <summary>
        /// Your API key - Locate at https://pixabay.com/api/docs/
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// A URL encoded search term. If omitted, all images are returned. This value may not exceed 100 characters. Example: "yellow flower"
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Language code of the language to be searched in. 
        /// Accepted values: cs, da, de, en, es, fr, id, it, hu, nl, no, pl, pt, ro, sk, fi, sv, tr, vi, th, bg, ru, el, ja, ko, zh 
        /// Default: "en"
        /// </summary>
        public EnumLanguage Language { get; set; } = EnumLanguage.en;

        /// <summary>
        /// ID, hash ID, or a comma separated list of values for retrieving specific images. In a comma separated list, IDs and hash IDs cannot be used together.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Choose between retrieving high resolution images and image details. When selecting details, you can access images up to a dimension of 960 x 720 px. 
        /// Default: Image_Details
        /// </summary>
        public EnumResponseGroup ResponseGroup { get; set; } = EnumResponseGroup.Image_Details;


        /// <summary>
        /// Filter results by image type.
        /// Default: All
        /// </summary>
        public EnumImageType ImageType { get; set; } = EnumImageType.All;

        /// <summary>
        /// Whether an image is wider than it is tall, or taller than it is wide. 
        /// Default: All
        /// </summary>
        public EnumOrientation Orientation { get; set; }

        /// <summary>
        /// Filter results by category. 
        /// </summary>
        public EnumCategory Category { get; set; }

        /// <summary>
        /// Minimum image width. 
        /// Disabled: 0
        /// </summary>
        public int MinWidth { get; set; } = 0;

        /// <summary>
        /// Minimum image height.  
        /// Disabled: 0
        /// </summary>
        public int MinHeight { get; set; } = 0;

        /// <summary>
        /// Select images that have received an Editor's Choice award.
        /// See https://pixabay.com/da/editors_choice/
        /// Default: false
        /// </summary>
        public bool EditorsChoice { get; set; } = false;

        /// <summary>
        /// A flag indicating that only images suitable for all ages should be returned. 
        /// Default: false
        /// </summary>
        public bool SafeSearch { get; set; } = false;

        /// <summary>
        /// Returned search results are paginated. Use this parameter to select the page number. 
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Determine the number of results per page. 
        /// Accepted values: 3 - 200 
        /// Default: 20
        /// </summary>
        public int PerPage { get; set; } = 20;

    }
}
