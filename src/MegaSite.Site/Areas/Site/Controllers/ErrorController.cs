using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Tools;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;
using MegaSite.Plugins;

namespace MegaSite.Site.Areas.Site.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Site/Error/

        private readonly PathResolver _pathResolver;
        private readonly SiteViewModel _vm;

        public ErrorController(IManagers managers, ActionPluginManager pluginManager, PathResolver pathResolver)
        {
            _pathResolver = pathResolver;
            _vm = new SiteViewModel(managers, pluginManager);
        }

        public ActionResult Index()
        {
            Mailer.Send("MegaSite", "Sistema", "Alerta de erro MegaSite", "Algum erro muito louco aconteceu!");
            _vm.Title = "Tratemento de erro";
            return View(GetHomeFile(), _vm);
        }

        #region PrivateMethods

        private string GetHomeFile()
        {
            var relPath = _pathResolver.CurrentThemeViews + "Error.cshtml";
            var path = Server.MapPath(relPath);

            if (System.IO.File.Exists(path))
            {
                return relPath;
            }
            return _pathResolver.CurrentThemeViews + "Index.cshtml";
        }

        #endregion
    }
}
