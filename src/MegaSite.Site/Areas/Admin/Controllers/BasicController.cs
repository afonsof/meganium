using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Managers;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    public class BasicController : BaseController
    {
        private readonly IManagers _managers;

        public BasicController(IManagers managers)
        {
            _managers = managers;
        }

        [Authorize]
        public ActionResult Index()
        {
            //Refactor: Que loucura é essa no controller???

            var options = _managers.LicenseManager.GetOptions();
            var vm = new BasicIndexVm
            {
                FacebookId = options.GetLong("FacebookId"),
                GoogleAnalyticsTracker = options.Get("GoogleAnalyticsTracker"),
                SiteTitle = options.Get("SiteTitle"),
                AdminEmail = options.Get("AdminEmail"),
                Theme = options.Get("Theme"),
                SiteDescription = options.Get("SiteDescription"),
                SiteLanguage = options.Get("SiteLanguage"),
                DefaultAlbumImportingPostTypeId = options.GetInt("DefaultAlbumImportingPostTypeId"),
                DefaultVideoImportingPostTypeId = options.GetInt("DefaultVideoImportingPostTypeId"),
                DefaultPostTypeId = options.GetInt("DefaultPostTypeId"),
                Color1 = options.Get("Color1"),
                Color2 = options.Get("Color2"),
                PostTypeSelect = new SelectList(_managers.PostTypeManager.GetAll(), "Id", "SingularName"),
                AllowImportMediaFiles = options.Get("AllowImportMediaFiles", false)
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(BasicIndexVm vm)
        {
            var options = _managers.LicenseManager.GetOptions();
            options.Set("FacebookId", vm.FacebookId);
            options.Set("GoogleAnalyticsTracker", vm.GoogleAnalyticsTracker);
            options.Set("SiteTitle", vm.SiteTitle);
            options.Set("AdminEmail", vm.AdminEmail);
            options.Set("Theme", vm.Theme);
            options.Set("SiteDescription", vm.SiteDescription);
            options.Set("DefaultAlbumImportingPostTypeId", vm.DefaultAlbumImportingPostTypeId);
            options.Set("DefaultVideoImportingPostTypeId", vm.DefaultVideoImportingPostTypeId);
            options.Set("DefaultPostTypeId", vm.DefaultPostTypeId);
            options.Set("Color1", vm.Color1);
            options.Set("Color2", vm.Color2);
            options.Set("SiteLanguage", vm.SiteLanguage);
            options.Set("AllowImportMediaFiles", vm.AllowImportMediaFiles);
            vm.PostTypeSelect = new SelectList(_managers.PostTypeManager.GetAll(), "Id", "SingularName");
            return View(vm);
        }
    }
}