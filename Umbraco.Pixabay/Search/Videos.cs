using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Pixabay.Extensions;
using static Umbraco.Pixabay.Enums;

namespace Umbraco.Pixabay.Search
{
    public static class Videos
    {

        public static Models.VideoSearchResults Search(string apikey,
                                        string searchTerm = "",
                                        EnumLanguage language = EnumLanguage.en,
                                        string id = "",
                                        EnumResponseGroup responseGroup = EnumResponseGroup.Image_Details,
                                        EnumVideoType videoType = EnumVideoType.All,
                                        EnumCategory category = EnumCategory.All,
                                        int minWidth = 0,
                                        int minHeight = 0,
                                        bool EditorsChoice = false,
                                        bool SafeSearch = false,
                                        int Page = 1,
                                        int PerPage = 20
                                        )
        {
            if (string.IsNullOrEmpty(apikey))
            {
                throw new System.ArgumentNullException("apiKey");
            }

            try
            {
                Models.VideoSearchOptions options = new Models.VideoSearchOptions()
                {
                    ApiKey = apikey,
                    SearchTerm = searchTerm,
                    Language = language,
                    Id = id,
                    VideoType = videoType,
                    Category = category,
                    MinWidth = minWidth,
                    MinHeight = minHeight,
                    EditorsChoice = EditorsChoice,
                    SafeSearch = SafeSearch,
                    Page = Page,
                    PerPage = PerPage
                };
                return SearchWithOptions(options);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static Models.VideoSearchResults SearchWithOptions(Models.VideoSearchOptions options)
        {


            Models.VideoSearchResults searchResult = null;

            if (options == null)
            {
                throw new System.ArgumentNullException("options");
            }

            try
            {
                System.Net.WebClient WebClient = new System.Net.WebClient();

                System.Text.StringBuilder query = new System.Text.StringBuilder();
                query.Append(string.Concat("key=", options.ApiKey));

                if (!string.IsNullOrEmpty(options.SearchTerm))
                {
                    query.Append(string.Concat("&q=", HttpUtility.UrlEncode(options.SearchTerm)));
                }

                if (options.Language != EnumLanguage.en)
                {
                    query.Append(string.Concat("&lang=", options.Language.GetName(EnumExtensions.StringCase.Lower)));
                }

                if (!string.IsNullOrEmpty(options.Id))
                {
                    query.Append(string.Concat("&id=", HttpUtility.UrlEncode(options.Id)));
                }

                if (options.VideoType != EnumVideoType.All)
                {
                    query.Append(string.Concat("&video_type=", options.VideoType.GetName(EnumExtensions.StringCase.Lower)));
                }

                if (options.Category != EnumCategory.All)
                {
                    query.Append(string.Concat("&category=", options.Category.GetName(EnumExtensions.StringCase.Lower)));
                }

                if (options.MinWidth > 0)
                {
                    query.Append(string.Concat("&min_width=", options.MinWidth.ToString()));
                }

                if (options.MinHeight > 0)
                {
                    query.Append(string.Concat("&min_height=", options.MinHeight.ToString()));
                }

                if (options.EditorsChoice)
                {
                    query.Append("&editors_choice=true");
                }

                if (options.SafeSearch)
                {
                    query.Append("&safesearch=true");
                }

                if (options.Page > 1)
                {
                    query.Append(string.Concat("&page=", options.Page.ToString()));
                }

                if (options.PerPage != 20)
                {

                    if (options.PerPage == 0)
                    {
                        options.PerPage = 20;
                    }

                    if (options.PerPage < 3)
                    {
                        options.PerPage = 3;
                    }

                    if (options.PerPage > 200)
                    {
                        options.PerPage = 200;
                    }

                    query.Append(string.Concat("&per_page=", options.PerPage.ToString()));
                }


                string url = string.Concat("https://pixabay.com/api/videos/?", query.ToString());
                string json = WebClient.DownloadString(url);

                searchResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.VideoSearchResults>(json);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return searchResult;

        }


    }
}
