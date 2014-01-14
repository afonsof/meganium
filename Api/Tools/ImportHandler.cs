using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Dongle.Reflection;
using Meganium.Api.Entities;
using Meganium.Api.Managers;
using Meganium.Api.Plugins;
using Meganium.Api.Trash;

namespace Meganium.Api.Tools
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
                    mf.ExtName = externalServiceName;
                    dbPost.FeaturedMediaFileJson = InternalJsonSerializer.Serialize(mf);
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
                    var dbMediaFiles = dbPost.MediaFiles.Where(m => m.ExtId == null);
                    mediaFilesList.AddRange(dbMediaFiles);
                }
                post.MediaFilesJson = InternalJsonSerializer.Serialize(mediaFilesList);
            }
        }
    }
}