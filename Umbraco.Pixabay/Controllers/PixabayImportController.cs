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

            if (importRequest.ImageEntries == null)
            {
                throw new System.ArgumentNullException("importRequest.IdList");
            }

            if (importRequest.MediaFolder == 0)
            {
                throw new System.ArgumentNullException("importRequest.MediaFolder");
            }

            IMedia mediaFolder = Services.MediaService.GetById(importRequest.MediaFolder);

            if (mediaFolder != null && importRequest.MediaType == 0)
            {
                // Instances new response object.
                importResponse = new UmbracoImportResponse();

                foreach (ImageEntry url in importRequest.ImageEntries)
                {
                    var stopWatch = System.Diagnostics.Stopwatch.StartNew();

                    try
                    {
                        SaveImageByUrl(url.WebformatURL, mediaFolder);
                        stopWatch.Stop();
                        importResponse.SuccessList.Add(new ImportEntry()
                        {
                            Filename = System.IO.Path.GetFileName(url.PreviewURL),
                            ImportTime = stopWatch.ElapsedMilliseconds
                        });

                    }
                    catch (System.Exception ex)
                    {

                        stopWatch.Stop();

                        Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Error ocurred while importing images.", ex);

                        importResponse.FailureList.Add(new ImportEntry()
                        {
                            Filename = System.IO.Path.GetFileName(url.PreviewURL),
                            ImportTime = stopWatch.ElapsedMilliseconds,
                            Error = ex.ToString()
                        });

                    }

                }
            }
            else
            {
                Logger.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, $"Import could not be performed, since the selected folder doesn't exist. Requested folder id: '{importRequest.MediaFolder}'.");
            }

            return importResponse;
        }


        [HttpPost]
        public UmbracoImportResponse ImportVideo([FromBody] UmbracoImportVideoRequest importRequest)
        {

            UmbracoImportResponse importResponse = null;

            if (importRequest.Videos == null)
            {
                throw new System.ArgumentNullException("importRequest.Videos");
            }

            if (importRequest.MediaFolder == 0)
            {
                throw new System.ArgumentNullException("importRequest.MediaFolder");
            }

            IMedia mediaFolder = Services.MediaService.GetById(importRequest.MediaFolder);

            if (mediaFolder != null && importRequest.MediaType == 1)
            {
                // Instances new response object.
                importResponse = new UmbracoImportResponse();

                foreach (VideoEntry entry in importRequest.Videos)
                {
                    var stopWatch = System.Diagnostics.Stopwatch.StartNew();

                    try
                    {
                        SaveVideoByUrl(entry, mediaFolder);
                        stopWatch.Stop();
                        importResponse.SuccessList.Add(new ImportEntry()
                        {
                            Filename = System.IO.Path.GetFileName(importRequest.Videos.First().Videos.Large.Url.Split('?')[0]),
                            ImportTime = stopWatch.ElapsedMilliseconds
                        });

                    }
                    catch (System.Exception ex)
                    {
                        stopWatch.Stop();

                        Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Error ocurred while importing videos.", ex);

                        importResponse.FailureList.Add(new ImportEntry()
                        {
                            Filename = System.IO.Path.GetFileName(importRequest.Videos.First().Videos.Large.Url.Split('?')[0]),
                            ImportTime = stopWatch.ElapsedMilliseconds,
                            Error = ex.ToString()
                        });

                    }

                }
            }
            else
            {
                Logger.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, $"Import could not be performed, since the selected folder doesn't exist. Requested folder id: '{importRequest.MediaFolder}'.");
            }

            return importResponse;
        }


        public bool SaveImageByUrl(string url, IMedia mediaFolder)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(url);

                IMedia media = Services.MediaService.CreateMediaWithIdentity(filename, mediaFolder, "Image");

                WebClient wc = new WebClient();
                MemoryStream memoryStream = new MemoryStream(wc.DownloadData(url.Replace("_640", "_960")));

                Stream streamCopy = memoryStream;
                media.SetValue("umbracoFile", filename, streamCopy);

                Services.MediaService.Save(media);
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Error ocurred while importing image.", ex);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Error ocurred while importing video.", ex);
                throw ex;
            }
        }

    }
}
