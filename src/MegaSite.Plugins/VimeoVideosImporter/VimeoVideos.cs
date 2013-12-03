using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Dongle.Serialization;
using MegaSite.Api;

namespace MegaSite.Plugins.VimeoVideosImporter
{
    public sealed class VimeoVideos : IImportPlugin
    {
        const string VimeoRegex = @"vimeo\.com\/(\d+)";
        const string VimeoApiRoot = @"http://vimeo.com/api/v2";

        public IEnumerable<ImportPost> ReadPosts(NameValueCollection values)
        {
            ValidateArguments(values);

            var address = String.Format("{0}/{1}/videos.json", VimeoApiRoot, values["username"]);
            var videos = GetVideosFromUrl(address);
            var posts = new List<ImportPost>();

            foreach (var vimeoVideo in videos)
            {
                posts.Add(new ImportPost
                {
                    CreatedAt = DateTime.Now,
                    PublishedAt = DateTime.Now,
                    Published = true,
                    Title = vimeoVideo.title,
                    Content = vimeoVideo.description,
                    FeaturedMediaFileJson = CreateMediaFileJson(vimeoVideo),
                    ExternalServiceId = vimeoVideo.id,
                    ExternalServiceUser = values["username"]
                });
            }
            return posts;
        }

        public IEnumerable<MediaFile> ReadMediaFiles(ImportPost post, NameValueCollection values)
        {
            return null;
        }

        public string CreateMediaFileJson(string videoId)
        {
            var match = Regex.Match(videoId, VimeoRegex);

            if (match.Captures.Count > 0)
            {
                var address = String.Format("{0}/video/{1}.json", VimeoApiRoot, match.Groups[1].Value);
                var videos = GetVideosFromUrl(address);
                if (videos != null && videos.Any())
                {
                    return CreateMediaFileJson(videos[0]);
                }
            }
            return null;
        }

        #region PrivateMethods

        private List<VimeoVideo> GetVideosFromUrl(string address)
        {
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(address);
                var videos = JsonSimpleSerializer.UnserializeFromString<List<VimeoVideo>>(json);
                return videos;
            }
        }

        private static void ValidateArguments(NameValueCollection values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (String.IsNullOrEmpty(values["username"]))
            {
                throw new ArgumentException("username");
            }
        }

        private string CreateMediaFileJson(VimeoVideo vimeoVideo)
        {
            return JsonSimpleSerializer.SerializeToString(new MediaFile
            {
                Title = vimeoVideo.title,
                Description = vimeoVideo.description,
                ExternalServiceId = vimeoVideo.id,
                Url = vimeoVideo.thumbnail_large,
            });
        }

        #endregion

        class VimeoVideo
        {
            public DateTime created_on { get; set; }
            public string thumbnail_large { get; set; }
            public string user_url { get; set; }
            public string id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
        }
    }
}