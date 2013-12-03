using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Dongle.Serialization;
using Google.YouTube;
using MegaSite.Api;

namespace MegaSite.Plugins.YoutubeVideosImporter
{
    public class YouTubeVideos : IImportPlugin
    {
        const string YouTubeUrlRegex = @"(?:(?:youtube.com(?:.[a-z]{2})?|youtu.be)/(?:watch\?(?:[\w_\-=&]+)?v=|embed\/)?)?([\w_\-]{11})";
        private readonly YouTubeRequest _request;

        public YouTubeVideos()
        {
            var settings = new YouTubeRequestSettings("MegaSite", "AI39si4CDP4K5cWp1AQyAnpEYVFVP1CBXmGr3uQxqlIhCR6d7k26PQm1JU8WJL4A21nrlL74KXtGqUvbDINabqTgEOwnqiMujg");
            _request = new YouTubeRequest(settings);
        }


        public IEnumerable<ImportPost> ReadPosts(NameValueCollection values)
        {
            ValidateArguments(values);

            var query = new Uri("http://gdata.youtube.com/feeds/api/users/" + values["username"] + "/uploads");
            var videos = GetVideosFromUri(query);

            return videos.Select(v => new ImportPost
            {
                CreatedAt = DateTime.Now,
                PublishedAt = v.Updated,
                Published = true,
                Title = v.Title,
                Content = v.Description,
                FeaturedMediaFileJson = CreateMediaFileJson(v),
                ExternalServiceId = v.VideoId,
                ExternalServiceUser = v.Uploader
            });
        }

        public IEnumerable<MediaFile> ReadMediaFiles(ImportPost post, NameValueCollection values)
        {
            return null;
        }

        public string CreateMediaFileJson(string videoId)
        {
            var match = Regex.Match(videoId, YouTubeUrlRegex);

            if (match.Captures.Count > 0)
            {
                var query = new Uri("http://gdata.youtube.com/feeds/api/videos/" + match.Groups[1].Value);
                var videos = GetVideosFromUri(query);

                if (videos.Any())
                {
                    return CreateMediaFileJson(videos.First());
                }
            }
            return null;
        }

        #region PrivateMethods

        private static void ValidateArguments(NameValueCollection values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (String.IsNullOrEmpty(values["username"]))
            {
                throw new ArgumentException("username");
            }
        }

        private List<Video> GetVideosFromUri(Uri uri)
        {
            return _request.Get<Video>(uri).Entries.ToList();
        }

        private string CreateMediaFileJson(Video youTubeVideo)
        {
            return JsonSimpleSerializer.SerializeToString(new MediaFile
            {
                Title = youTubeVideo.Title,
                Description = youTubeVideo.Description,
                ExternalServiceId = youTubeVideo.VideoId,
                Url = youTubeVideo
                    .Thumbnails
                    .OrderByDescending(t => t.Width)
                    .FirstOrDefault().Url
            });
        }

        #endregion
    }
}