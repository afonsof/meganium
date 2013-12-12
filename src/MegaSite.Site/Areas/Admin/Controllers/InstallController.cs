using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Dongle.Serialization;
using MegaSite.Api;
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
            initializer.Initialize(vm.Theme, configExchange);
            return RedirectToAction("Index", "Post");
        }

        private ConfigExchange LoadConfigExchange(InstallResetThemeVm model)
        {
            var pathResolver = new PathResolver(Options.Instance);
            var path = Server.MapPath(pathResolver.Themes + model.Theme);

            var configExchange = JsonSimpleSerializer.UnserializeFromFile<ConfigExchange>(new FileInfo(path + "\\theme.json"));
            return configExchange;
        }
    }
}