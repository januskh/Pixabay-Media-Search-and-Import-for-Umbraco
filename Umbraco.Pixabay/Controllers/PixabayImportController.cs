using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Pixabay.Models;
using Umbraco.Pixabay.Search;
using Umbraco.Web.WebApi;

namespace Umbraco.Pixabay.Controllers
{
    public class PixabayImportController : UmbracoAuthorizedApiController
    {
        //public static string ApiKey = "8070652-c65c9e408eabd62c9e8467431";
        internal Models.Settings settings = null;

        public PixabayImportController()
        {
            PixabaySettingsController settings = new PixabaySettingsController();
            this.settings = settings.GetSettings();
        }


        [HttpPost]
        public UmbracoImportResponse Import([FromBody] UmbracoImportRequest importRequest)
        {

            UmbracoImportResponse importResponse = null;

            if (importRequest.IdList == null)
            {
                throw new System.ArgumentNullException("importRequest.IdList");
            }

            if (importRequest.MediaFolder == 0)
            {
                throw new System.ArgumentNullException("importRequest.MediaFolder");
            }

            IMedia mediaFolder = Services.MediaService.GetById(importRequest.MediaFolder);

            if (importRequest.MediaType == 0)
            {
                // Image import

                Models.ImageSearchOptions options = new Models.ImageSearchOptions()
                {
                    ApiKey = settings.ApiKey
                };
                options.Id = string.Join(",", importRequest.IdList);
                ImageSearchResult searchResult = Images.SearchWithOptions(options);

                if (searchResult != null && searchResult.Images != null)
                {

                    importResponse = new UmbracoImportResponse();

                    foreach (ImageEntry entry in searchResult.Images)
                    {
                        var stopWatch = System.Diagnostics.Stopwatch.StartNew();

                        try
                        {
                            SaveImageByUrl(entry, mediaFolder);
                            stopWatch.Stop();
                            importResponse.SuccessList.Add(new ImportEntry()
                            {
                                Filename = System.IO.Path.GetFileName(entry.PreviewURL),
                                ImportTime = stopWatch.ElapsedMilliseconds
                            });

                        }
                        catch (System.Exception ex)
                        {
                            stopWatch.Stop();
                            importResponse.FailureList.Add(new ImportEntry()
                            {
                                Filename = System.IO.Path.GetFileName(entry.PreviewURL),
                                ImportTime = stopWatch.ElapsedMilliseconds,
                                Error = ex.ToString()
                            });

                        }

                    }
                }
            }
            else
            {
                // Video import
                Models.VideoSearchOptions options = new Models.VideoSearchOptions()
                {
                    ApiKey = settings.ApiKey
                };
                options.Id = string.Join(",", importRequest.IdList);
                VideoSearchResults searchResult = Videos.SearchWithOptions(options);

                if (searchResult != null && searchResult.Videos != null)
                {

                    importResponse = new UmbracoImportResponse();

                    foreach (VideoEntry entry in searchResult.Videos)
                    {
                        var stopWatch = System.Diagnostics.Stopwatch.StartNew();

                        try
                        {
                            SaveVideoByUrl(entry, mediaFolder);
                            stopWatch.Stop();
                            importResponse.SuccessList.Add(new ImportEntry()
                            {
                                Filename = System.IO.Path.GetFileName(searchResult.Videos.First().Videos.Large.Url.Split('?')[0]),
                                ImportTime = stopWatch.ElapsedMilliseconds
                            });

                        }
                        catch (System.Exception ex)
                        {
                            stopWatch.Stop();
                            importResponse.FailureList.Add(new ImportEntry()
                            {
                                Filename = System.IO.Path.GetFileName(searchResult.Videos.First().Videos.Large.Url.Split('?')[0]),
                                ImportTime = stopWatch.ElapsedMilliseconds,
                                Error = ex.ToString()
                            });

                        }

                    }
                }

            }

            return importResponse;
        }


        public bool SaveImageByUrl(ImageEntry entry, IMedia mediaFolder)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(entry.PreviewURL);

                IMedia media = Services.MediaService.CreateMediaWithIdentity(filename, mediaFolder, "Image");

                WebClient wc = new WebClient();
                MemoryStream memoryStream = new MemoryStream(wc.DownloadData(entry.WebformatURL));

                Stream streamCopy = memoryStream;
                media.SetValue("umbracoFile", filename, streamCopy);

                Services.MediaService.Save(media);
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveVideoByUrl(VideoEntry entry, IMedia mediaFolder)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(entry.Videos.Large.Url.Split('?')[0]);

                IMedia media = Services.MediaService.CreateMediaWithIdentity(filename, mediaFolder, "File");

                WebClient wc = new WebClient();
                MemoryStream memoryStream = new MemoryStream(wc.DownloadData(entry.Videos.Large.Url));

                Stream streamCopy = memoryStream;
                media.SetValue("umbracoFile", filename, streamCopy);

                Services.MediaService.Save(media);
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
