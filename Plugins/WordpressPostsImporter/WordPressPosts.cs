using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using Dongle.System;
using Meganium.Api;
using Meganium.Api.Plugins;

namespace Meganium.Plugins.WordpressPostsImporter
{
    public class WordPressPosts /*todo: IImportPlugin*/
    {
        public IEnumerable<ImportPost> ReadPosts(NameValueCollection values)
        {
            using (var textReader = new StringReader(values["file"]))
            {
                var reader = XmlReader.Create(textReader);
                var feed = SyndicationFeed.Load(reader);
                if (feed == null)
                {
                    yield return null;
                }
                foreach (var wpPost in GetWpPosts(feed))
                {
                    ImportPost post;

                    if (IsPage(wpPost))
                    {
                        post = new ImportPost();
                    }
                    else
                    {
                        post = new ImportPost
                        {
                            /*todo: PostType = typePost,*/
                            CategoriesStr = GetCategories(wpPost).ToCsv()
                        };
                    }

                    post.Title = wpPost.Title.Text;
                    post.CreatedAt = wpPost.PublishDate.DateTime;
                    post.PublishedAt = wpPost.PublishDate.DateTime;
                    post.Published = true;
                    post.ExternalServiceId = GetWpId(wpPost);
                    post.ExternalServiceUser = post.Content = GetContent(wpPost);
                    post.CreatedByStr = GetCreatedBy(wpPost);

                    if (IsPage(wpPost))
                    {
                        FillParentPage(post, wpPost);
                    }
                    yield return post;
                }
            }
        }

        public IEnumerable<MediaFile> ReadMediaFiles(ImportPost post, NameValueCollection values)
        {
            return null;
        }

        #region PrivateMethods

        private void FillParentPage(ImportPost post, SyndicationItem wpPost)
        {
            post.ParentStr = GetParentPageId(wpPost);
        }

        private static IEnumerable<SyndicationItem> GetWpPosts(SyndicationFeed feed)
        {
            var wpPosts =
                feed.Items
                    .Where(
                        f =>
                        (f.ElementExtensions.Any(ex => ex.OuterName == "post_type" && ex.GetObject<string>() == "post") ||
                         f.ElementExtensions.Any(ex => ex.OuterName == "post_type" && ex.GetObject<string>() == "page"))
                        &&
                        f.ElementExtensions.Count(ex => ex.OuterName == "status" && ex.GetObject<string>() == "publish") >
                        0);
            return wpPosts;
        }

        private static string GetContent(SyndicationItem wpPost)
        {
            var contentElement = wpPost.ElementExtensions.FirstOrDefault(ex => ex.OuterName == "encoded");
            var str = contentElement != null ? contentElement.GetObject<string>() : "";

            return "<p>" + Regex.Replace(str, "\r?\n\r?\n", "\r\n</p>\r\n<p>") + "</p>";
        }

        private string GetCreatedBy(SyndicationItem wpPost)
        {
            var authorElement = wpPost.ElementExtensions.FirstOrDefault(ex => ex.OuterName == "creator");
            if (authorElement != null)
            {
                return authorElement.GetObject<string>();
            }
            return null;
        }

        private static string GetParentPageId(SyndicationItem wpPost)
        {
            var parentElement = wpPost.ElementExtensions.FirstOrDefault(ex => ex.OuterName == "post_parent");

            if (parentElement == null)
            {
                return null;
            }
            return parentElement.GetObject<string>();
        }

        private static string GetWpId(SyndicationItem wpPost)
        {
            var wpIdObj = wpPost.ElementExtensions.FirstOrDefault(ex => ex.OuterName == "post_id");
            if (wpIdObj == null)
            {
                return null;
            }
            return wpIdObj.GetObject<string>();
        }

        private static bool IsPage(SyndicationItem wpPost)
        {
            return wpPost.ElementExtensions.Any(ex => ex.OuterName == "post_type" && ex.GetObject<string>() == "page");
        }

        private static IEnumerable<string> GetCategories(SyndicationItem wpPost)
        {
            return wpPost.Categories.Where(c => c.Scheme == "category").Select(c => c.Name);
        }

        #endregion
    }
}