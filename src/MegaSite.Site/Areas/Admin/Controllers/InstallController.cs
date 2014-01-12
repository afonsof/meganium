using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Dongle.Serialization;
using MegaSite.Api.Managers;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Installer;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    public class InstallController : BaseController
    {
        private readonly IManagers _managers;

        public InstallController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult ResetTheme()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetTheme(InstallResetThemeVm vm)
        {
            if (vm.Password != ConfigurationManager.AppSettings["MasterPassword"])
            {
                return null;
            }
            var initializer = new Initializer(_managers);

            if (vm.ReinitializeDatabase)
            {
                initializer.ReinitializeDatabase();
            }

            var configExchange = LoadConfigExchange(vm);
            initializer.Initialize(vm.License, configExchange);
            return RedirectToAction("Index", "Post");
        }

        private ConfigExchange LoadConfigExchange(InstallResetThemeVm model)
        {
            var pathResolver = new PathResolver(_managers.License.Options);
            var path = Server.MapPath(pathResolver.Licenses + model.License);

            var configExchange = JsonSimpleSerializer.UnserializeFromFile<ConfigExchange>(new FileInfo(path + "\\license.json"));
            return configExchange;
        }
    }
}