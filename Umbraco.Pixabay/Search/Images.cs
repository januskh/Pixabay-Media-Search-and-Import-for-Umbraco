using System;
using System.Collections.Generic;
using System.Web;
using Umbraco.Pixabay.Extensions;
using static Umbraco.Pixabay.Enums;

namespace Umbraco.Pixabay.Search
{
    public static class Images
    {

        public static Models.ImageSearchResult Search(string apikey,
                                                string searchTerm = "",
                                                EnumLanguage language = EnumLanguage.en,
                                                string id = "",
                                                EnumResponseGroup responseGroup = EnumResponseGroup.Image_Details,
                                                EnumImageType imageType = EnumImageType.All,
                                                EnumOrientation orientation = EnumOrientation.All,
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
                Models.ImageSearchOptions options = new Models.ImageSearchOptions() { ApiKey = apikey,
                                                                            SearchTerm = searchTerm,
                                                                            Language = language,
                                                                            Id = id,
                                                                            ResponseGroup = responseGroup,
                                                                            ImageType = imageType,
                                                                            Orientation = orientation,
                                                                            Category = category,
                                                                            MinWidth = minWidth,
                                                                            MinHeight = minHeight,
                                                                            EditorsChoice = EditorsChoice,
                                                                            SafeSearch = SafeSearch,
                                                                            Page = Page,
                                                                            PerPage = PerPage };
                return SearchWithOptions(options);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static Models.ImageSearchResult SearchWithOptions(Models.ImageSearchOptions options)
        {


            Models.ImageSearchResult searchResult = null;

            if (options == null)
            {
                throw new System.ArgumentNullException("options");
            }

            System.Text.StringBuilder query = new System.Text.StringBuilder();
            try
            {
                System.Net.WebClient WebClient = new System.Net.WebClient();

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

                if (options.ResponseGroup != EnumResponseGroup.Image_Details)
                {
                    query.Append(string.Concat("&response_group=", options.ResponseGroup.GetName(EnumExtensions.StringCase.Lower)));
                }

                if (options.ImageType != EnumImageType.All)
                {
                    query.Append(string.Concat("&image_type=", options.ImageType.GetName(EnumExtensions.StringCase.Lower)));
                }

                if (options.Orientation != EnumOrientation.All)
                {
                    query.Append(string.Concat("&orientation=", options.Orientation.GetName(EnumExtensions.StringCase.Lower)));
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


                string url = string.Concat("https://pixabay.com/api/?", query.ToString());
                string json = WebClient.DownloadString(url);

                searchResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ImageSearchResult>(json);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return searchResult;

        }

    }
}
