using System;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Dongle.System;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Api.Resources;

namespace MegaSite.Site.Areas.Site.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IManagers _managers;
        private readonly PathResolver _pathResolver;
        private readonly SiteViewModel _vm;

        public HomeController(IManagers managers, PathResolver pathResolver)
        {
            _managers = managers;
            _pathResolver = pathResolver;
            _vm = new SiteViewModel(managers);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            _vm.Title = Options.Instance.Get("SiteTitle");
            return View(GetHomeFile(), _vm);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult SlugShow(int postTypeId, string slug)
        {
            if (String.IsNullOrEmpty(slug))
            {
                return new HttpNotFoundResult();
            }
            _vm.CurrentPost = _managers.PostManager.GetPublishedBySlugAndPostType(slug, postTypeId);

            if (_vm.CurrentPost == null)
            {
                return new HttpNotFoundResult();
            }
            _vm.Title = Options.Instance.Get("SiteTitle") + " » " + _vm.CurrentPost.Title;
            return View(GetShowFile(_vm.CurrentPost), _vm);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult SlugIndex(int postTypeId)
        {
            var postType = _managers.PostTypeManager.GetById(postTypeId);
            if (postType == null)
            {
                return new HttpNotFoundResult();
            }
            _vm.CurrentPosts = _managers.PostManager.GetPublishedByPostType(postTypeId);
            _vm.Title = Options.Instance.Get("SiteTitle") + " » " + postType.PluralName;
            return View(GetIndexFile(postType), _vm);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult SlugCategory(int postTypeId, string slug)
        {
            var postType = _managers.PostTypeManager.GetById(postTypeId);
            if (postType == null)
            {
                return new HttpNotFoundResult();
            }

            var category = _managers.CategoryManager.GetBySlugAndPostType(slug, postTypeId);
            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            _vm.CurrentPosts = _managers.PostManager.GetPublishedByCategoryAndPostType(category, postTypeId);
            _vm.CurrentCategory = category;

            _vm.Title = Options.Instance.Get("SiteTitle") + " » " + String.Format("{0} {1} {2}", postType.PluralName, Resource.Of, category.Title);

            return View(GetCategoryFile(category), _vm);
        }

        //TODO:
        /*public ActionResult SlugPluginAction(string pluginName, string pluginAction)
        {
            _vm.Plugin.RunAction(pluginName, pluginAction, HttpContext);
            return null;
        }*/

        #region PrivateMethods

        private string GetShowFile(Post post)
        {
            var postTypeSlug = post.PostType.SingularName.ToSlug();

            var relpath = _pathResolver.CurrentThemeViews + "Show-" + postTypeSlug + "-" + post.Slug + ".cshtml";
            string path = Server.MapPath(relpath);
            if (System.IO.File.Exists(path))
            {
                return relpath;
            }

            relpath = _pathResolver.CurrentThemeViews + "Show-" + postTypeSlug + ".cshtml";
            path = Server.MapPath(relpath);
            if (System.IO.File.Exists(path))
            {
                return relpath;
            }
            return _pathResolver.CurrentThemeViews + "Show.cshtml";
        }

        private string GetIndexFile(PostType type)
        {
            var relPath = _pathResolver.CurrentThemeViews;

            relPath += "Index-" + type.SingularName.ToSlug() + ".cshtml";

            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return _pathResolver.CurrentThemeViews + "Index.cshtml";
        }

        private string GetHomeFile()
        {
            var relPath = _pathResolver.CurrentThemeViews + "Home.cshtml";
            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return _pathResolver.CurrentThemeViews + "Index.cshtml";
        }

        private string GetCategoryFile(Category category)
        {
            var relPath = _pathResolver.CurrentThemeViews;

            relPath += "Index-" + category.PostType.SingularName.ToSlug() + "-" + category.Slug + ".cshtml";

            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return GetIndexFile(category.PostType);
        }

        #endregion

    }
}