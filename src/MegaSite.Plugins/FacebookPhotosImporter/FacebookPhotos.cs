using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MegaSite.Api;

namespace MegaSite.Plugins.FacebookPhotosImporter
{
    public class FacebookPhotos// : IImportPlugin
    {
        /*private const string FacebookOauthUrl = "https://www.facebook.com/dialog/oauth";
        private const string FacebookFormat = "{0}?client_id={1}&redirect_uri={2}?back=true&response_type=token&scope=user_photos,manage_pages,friends_photos";
        private const string FacebookScriptRedirect = "<script>window.location='{0}?' + window.location.hash.substr(1);</script>";
        private IRestClient _restClient;

        public IEnumerable<ImportPost> ReadPosts(NameValueCollection values)
        {
            var accessToken = values["access_token"];
            const string query = "SELECT object_id,created,name,location,size,cover_object_id,owner from album WHERE visible=\"everyone\" AND owner=me() or owner in (select page_id from page_admin where uid=me())";
            var response = ExecuteQuery<FbList<FbAlbum>>(query, accessToken);
            var albums = response.Data.data;
            return albums.Select(a => ConvertAlbumToPost(a, accessToken)).ToList();
        }

        public IEnumerable<MediaFile> ReadMediaFiles(ImportPost post, NameValueCollection values)
        {
            var accessToken = values["access_token"];
            var query = "SELECT object_id,src_small,src,src_big,created FROM photo WHERE album_object_id=\"" +
                        post.ExternalServiceId + "\"";
            var response = ExecuteQuery<FbList<FbPhoto>>(query, accessToken);
            var photos = response.Data.data;

            return photos.Select(ConvertPhotoToMediaFile).ToList();
        }

        private ImportPost ConvertAlbumToPost(FbAlbum album, string accessToken)
        {
            var post = new ImportPost
            {
                ExternalServiceId = album.object_id,
                CreatedAt = album.created,
                Title = album.name,
                PublishedAt = album.created,
                Published = true,
                Location = album.location,
                ExternalServiceUser = album.owner,
                MediaFilesCount = album.size
            };

            var query = "SELECT object_id, src_big, created FROM photo WHERE object_id=\"" + album.cover_object_id + "\"";
            var restResponse = ExecuteQuery<FbList<FbPhoto>>(query, accessToken);
            var photo = restResponse.Data.data.FirstOrDefault();
            if (photo != null)
            {
                post.FeaturedMediaFileJson = JsonSimpleSerializer.SerializeToString(new MediaFile
                {
                    Title = "",
                    Url = photo.src_big,
                    ExternalServiceId = photo.object_id,
                });
            }
            return post;
        }

        #region PrivateMethods

        private static MediaFile ConvertPhotoToMediaFile(FbPhoto fbPhoto)
        {
            var photo = new MediaFile
            {
                Title = "",
                Url = fbPhoto.src_big,
                ThumbUrl = fbPhoto.src,
                ExternalServiceId = fbPhoto.object_id,
            };
            return photo;
        }

        private IRestResponse<TEntity> ExecuteQuery<TEntity>(string query, string accessToken) where TEntity : new()
        {
            if (_restClient == null)
            {
                _restClient = new RestClient("https://graph.facebook.com");
            }
            var request = new RestRequest("fql");
            request.AddParameter("q", query);
            request.AddParameter("access_token", accessToken);
            return _restClient.Execute<TEntity>(request);
        }
        
        #endregion
        */
        #region Classes
        public class FbAlbum
        {
            public string object_id { get; set; }
            public DateTime created { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public uint size { get; set; }
            public string cover_object_id { get; set; }
            public string owner { get; set; }
        }

        public class FbList<TEntity>
        {
            public List<TEntity> data { get; set; }
        }

        public class FbPhoto
        {
            public string src { get; set; }
            public DateTime created { get; set; }
            public string src_small { get; set; }
            public string object_id { get; set; }
            public string src_big { get; set; }
        }

        #endregion
    }
}