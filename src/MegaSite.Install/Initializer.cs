using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Dongle.Reflection;
using Dongle.System;
using Dongle.System.IO;
using MegaSite.Api;
using MegaSite.Api.Entities;

namespace MegaSite.Installer
{
    public class Initializer
    {
        private readonly IManagers _managers;

        public Initializer(IManagers managers)
        {
            _managers = managers;
        }

        public void Initialize(string theme, ConfigExchange configExchange)
        {
            InitRootUser(configExchange);
            InitPostTypes(configExchange);
            InitOptions(theme, configExchange);
        }

        public void ReinitializeDatabase()
        {
            var root = new DirectoryInfo(ApplicationPaths.RootDirectory);
            var i = 0;
            var found = false;

            try
            {
                while (!found && i < 10)
                {
                    i++;
                    root = root.Parent;
                    if (root.GetFiles("web.config").Length > 0)
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    var rootPath = root.FullName;
                    Directory.Delete(rootPath + "\\Content\\Uploads\\Files", true);
                    Directory.Delete(rootPath + "\\Content\\Uploads\\Thumbs", true);
                }
            }
            catch
            {
            }
            NHibernateBuilder.Reset(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        #region Private Methods

        private void InitOptions(string theme, ConfigExchange configExchange)
        {
            _managers.LicenseManager.GetOptions().Set("Theme", theme);
            foreach (var option in configExchange.Options)
            {
                var value = option.Value;

                var match = Regex.Match(option.Value, @"@(\w+)\[(\w+)\]");
                if (match.Captures.Count > 0)
                {
                    if (match.Groups[1].Value.ToLowerInvariant() == "posttype")
                    {
                        var postType = _managers.PostTypeManager.GetBySingularName(match.Groups[2].Value);
                        if (postType != null)
                        {
                            value = postType.Id.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }
                _managers.LicenseManager.GetOptions().Set(option.Key, value);
            }
        }

        private void InitPostTypes(ConfigExchange configExchange)
        {
            foreach (var postTypeExchange in configExchange.PostTypes)
            {
                var postType = _managers.PostTypeManager.GetBySingularName(postTypeExchange.SingularName);
                if (postType != null)
                {
                    var id = postType.Id;
                    ObjectFiller<PostTypeExchange, PostType>.Merge(postTypeExchange, postType);
                    postType.Id = id;
                    _managers.PostTypeManager.Change(null, postType);
                }
                else
                {
                    postType = ObjectFiller<PostTypeExchange, PostType>.Fill(postTypeExchange);
                    _managers.PostTypeManager.CreateAndSave(null, postType);
                }

                if (postTypeExchange.Categories != null)
                {
                    foreach (var category in postTypeExchange.Categories)
                    {
                        _managers.CategoryManager.CreateAndSave(new Category
                        {
                            Title = category,
                            Slug = category.ToSlug(),
                            PostType = postType
                        });
                    }
                }

                if (postTypeExchange.Posts != null)
                {
                    foreach (var postExchange in postTypeExchange.Posts)
                    {
                        var categoryIds = new List<int>();
                        if (postExchange.Categories != null)
                        {
                            foreach (var title in postExchange.Categories)
                            {
                                var category = _managers.CategoryManager.GetByTitleAndPostType(title, postType);
                                if (category != null)
                                {
                                    categoryIds.Add(category.Id);
                                }
                            }
                        }
                        var post = ObjectFiller<PostExchange, Post>.Fill(postExchange);
                        _managers.PostManager.CreateAndSave(post, user: configExchange.RootUser, postType: postType, categoriesIds: categoryIds);
                    }
                }
            }
        }

        private void InitRootUser(ConfigExchange configExchange)
        {
            if (configExchange.RootUser != null)
            {
                _managers.UserManager.CreateAndSave(configExchange.RootUser);
            }
        }

        #endregion
    }
}
