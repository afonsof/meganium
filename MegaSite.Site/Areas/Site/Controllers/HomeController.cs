using System;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Dongle.System;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Api.Resources;
using MegaSite.Plugins;

namespace MegaSite.Site.Areas.Site.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IManagers _managers;
        private readonly SiteViewModel _vm;

        public HomeController(IManagers managers, ActionPluginManager pluginManager)
        {
            _managers = managers;
            _vm = new SiteViewModel(managers, pluginManager);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            var pathResolver = new PathResolver(_managers.License.Options);
            _vm.Title = _managers.License.Options.Get("SiteTitle");
            return View(GetHomeFile(pathResolver), _vm);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult SlugShow(int postTypeId, string slug)
        {
            var pathResolver = new PathResolver(_managers.License.Options);
            if (String.IsNullOrEmpty(slug))
            {
                return new HttpNotFoundResult();
            }
            _vm.CurrentPost = _managers.PostManager.GetPublishedBySlugAndPostType(slug, postTypeId);

            if (_vm.CurrentPost == null)
            {
                return new HttpNotFoundResult();
            }
            _vm.Title = _managers.License.Options.Get("SiteTitle") + " » " + _vm.CurrentPost.Title;
            return View(GetShowFile(_vm.CurrentPost, pathResolver), _vm);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult SlugIndex(int postTypeId)
        {
            var pathResolver = new PathResolver(_managers.License.Options);
            var postType = _managers.PostTypeManager.GetById(postTypeId);
            if (postType == null)
            {
                return new HttpNotFoundResult();
            }
            _vm.CurrentPosts = _managers.PostManager.GetPublishedByPostType(postTypeId);
            _vm.Title = _managers.License.Options.Get("SiteTitle") + " » " + postType.PluralName;
            return View(GetIndexFile(postType, pathResolver), _vm);
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult SlugCategory(int postTypeId, string slug)
        {
            var pathResolver = new PathResolver(_managers.License.Options);

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

            _vm.Title = _managers.License.Options.Get("SiteTitle") + " » " + String.Format("{0} {1} {2}", postType.PluralName, Resource.Of, category.Title);

            return View(GetCategoryFile(category, pathResolver), _vm);
        }

        public ActionResult SlugPluginAction(string pluginName, string pluginAction)
        {
            _vm.PluginManager.RunAction(pluginName, pluginAction, HttpContext);
            return null;
        }

        public ActionResult Error()
        {
            ViewBag.Title = "Algo de errado aconteceu!";
            return View();
        }

        #region PrivateMethods

        private string GetShowFile(Post post, PathResolver pathResolver)
        {
            var postTypeSlug = post.PostType.SingularName.ToSlug();

            var relpath = pathResolver.CurrentThemeViews + "Show-" + postTypeSlug + "-" + post.Slug + ".cshtml";
            string path = Server.MapPath(relpath);
            if (System.IO.File.Exists(path))
            {
                return relpath;
            }

            relpath = pathResolver.CurrentThemeViews + "Show-" + postTypeSlug + ".cshtml";
            path = Server.MapPath(relpath);
            if (System.IO.File.Exists(path))
            {
                return relpath;
            }
            return pathResolver.CurrentThemeViews + "Show.cshtml";
        }

        private string GetIndexFile(PostType type, PathResolver pathResolver)
        {
            var relPath = pathResolver.CurrentThemeViews;

            relPath += "Index-" + type.SingularName.ToSlug() + ".cshtml";

            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return pathResolver.CurrentThemeViews + "Index.cshtml";
        }

        private string GetHomeFile(PathResolver pathResolver)
        {
            var relPath = pathResolver.CurrentThemeViews + "Home.cshtml";
            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return pathResolver.CurrentThemeViews + "Index.cshtml";
        }

        private string GetCategoryFile(Category category, PathResolver pathResolver)
        {
            var relPath = pathResolver.CurrentThemeViews;

            relPath += "Index-" + category.PostType.SingularName.ToSlug() + "-" + category.Slug + ".cshtml";

            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return GetIndexFile(category.PostType, pathResolver);
        }

        #endregion

    }
}