using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Dongle.Reflection;
using Dongle.Serialization;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Plugins;

namespace MegaSite.Api.Tools
{
    public class ImportHandler
    {
        private readonly IImportPlugin _importPlugin;
        public ImportHandler(IImportPlugin importPlugin)
        {
            _importPlugin = importPlugin;
        }

        public IEnumerable<ImportPost> ReadAndCheckPosts(NameValueCollection values, IManagers managers)
        {
            var posts = _importPlugin.ReadPosts(values).ToList();
            var externalServiceName = _importPlugin.GetType().Name;

            foreach (var post in posts)
            {
                var dbPost = managers.PostManager.GetByExternalService(post.ExternalServiceId, externalServiceName);
                if (dbPost != null)
                {
                    post.Id = dbPost.Id;
                }
            }
            return posts;
        }

        public void ImportSelectedPosts(IEnumerable<int> indexes, IEnumerable<ImportPost> posts, NameValueCollection values, IManagers managers, User user, PostType postType)
        {
            var postList = posts.ToList();
            var selectedPosts = indexes.Select(i => postList[i]);
            var externalServiceName = _importPlugin.GetType().Name;

            foreach (var post in selectedPosts)
            {
                var dbPost = managers.PostManager.GetByExternalService(post.ExternalServiceId, externalServiceName);
                ImportMediaFiles(post, dbPost, values);

                if (dbPost == null)
                {
                    dbPost = ObjectFiller<ImportPost, Post>.Fill(post);
                }
                else
                {
                    var id = dbPost.Id;
                    ObjectFiller<ImportPost, Post>.Merge(post, dbPost);
                    dbPost.Id = id;
                }
                dbPost.ExternalServiceName = externalServiceName;

                if (dbPost.FeaturedMediaFileJson != null)
                {
                    var mf = dbPost.FeaturedMediaFile;
                    mf.ExternalServiceName = externalServiceName;
                    dbPost.FeaturedMediaFileJson = JsonSimpleSerializer.SerializeToString(mf);
                }
                if (dbPost.Id == 0)
                {
                    managers.PostManager.CreateAndSave(dbPost, user, postType);
                }
                else
                {
                    managers.PostManager.Change(dbPost, user);
                }
            }
        }

        private void ImportMediaFiles(ImportPost post, Post dbPost, NameValueCollection values)
        {
            var mediaFiles = _importPlugin.ReadMediaFiles(post, values);

            if (mediaFiles != null)
            {
                var mediaFilesList = mediaFiles.ToList();

                if (dbPost != null && dbPost.MediaFiles != null)
                {
                    //mescla com fotos locais
                    var dbMediaFiles = dbPost.MediaFiles.Where(m => m.ExternalServiceId == null);
                    mediaFilesList.AddRange(dbMediaFiles);
                }
                post.MediaFilesJson = JsonSimpleSerializer.SerializeToString(mediaFilesList);
            }
        }
    }
}