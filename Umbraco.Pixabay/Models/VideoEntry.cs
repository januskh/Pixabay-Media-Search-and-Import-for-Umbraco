using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay.Models
{
    public class VideoEntry
    {
        [JsonProperty("picture_id")]
        public string PictureId { get; set; }

        [JsonProperty("videos")]
        public Video Videos { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("downloads")]
        public int Downloads { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("favorites")]
        public int Favorites { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("comments")]
        public int Comments { get; set; }

        [JsonProperty("userImageURL")]
        public string UserImageURL { get; set; }

        [JsonProperty("pageURL")]
        public string PageURL { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("previewURL")]
        public string PreviewURL {
            get {
                // This value may be used to retrieve static preview images of the video in various sizes: 
                // https://i.vimeocdn.com/video/{ PICTURE_ID }_{ SIZE }.jpg 
                // Available sizes: 100x75, 200x150, 295x166, 640x360, 960x540, 1920x1080
                // Exampe: https://i.vimeocdn.com/video/529927645_295x166.jpg
                return $"https://i.vimeocdn.com/video/{ PictureId }_295x166.jpg";
            }
        }

    }

    public class Large
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class Small
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class Medium
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class Tiny
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class Video
    {

        [JsonProperty("large")]
        public Large Large { get; set; }

        [JsonProperty("small")]
        public Small Small { get; set; }

        [JsonProperty("medium")]
        public Medium Medium { get; set; }

        [JsonProperty("tiny")]
        public Tiny Tiny { get; set; }
    }


}
