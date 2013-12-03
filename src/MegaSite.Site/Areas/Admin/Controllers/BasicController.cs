using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    public class BasicController : BaseController
    {
        private readonly IManagers _managers;
        private readonly IOptions _options;

        public BasicController(IManagers managers, IOptions options)
        {
            _managers = managers;
            _options = options;
        }

        [Authorize]
        public ActionResult Index()
        {
            //Refactor: Que loucura é essa no controller???
            var vm = new BasicIndexVm
            {
                FacebookId = _options.GetLong("FacebookId"),
                GoogleAnalyticsTracker = _options.Get("GoogleAnalyticsTracker"),
                SiteTitle = _options.Get("SiteTitle"),
                AdminEmail = _options.Get("AdminEmail"),
                Theme = _options.Get("Theme"),
                SiteDescription = _options.Get("SiteDescription"),
                SiteLanguage = _options.Get("SiteLanguage"),
                DefaultAlbumImportingPostTypeId = _options.GetInt("DefaultAlbumImportingPostTypeId"),
                DefaultVideoImportingPostTypeId = _options.GetInt("DefaultVideoImportingPostTypeId"),
                DefaultPostTypeId = _options.GetInt("DefaultPostTypeId"),
                Color1 = _options.Get("Color1"),
                Color2 = _options.Get("Color2"),
                PostTypeSelect = new SelectList(_managers.PostTypeManager.GetAll(), "Id", "SingularName")
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(BasicIndexVm vm)
        {
            _options.Set("FacebookId", vm.FacebookId);
            _options.Set("GoogleAnalyticsTracker", vm.GoogleAnalyticsTracker);
            _options.Set("SiteTitle", vm.SiteTitle);
            _options.Set("AdminEmail", vm.AdminEmail);
            _options.Set("Theme", vm.Theme);
            _options.Set("SiteDescription", vm.SiteDescription);
            _options.Set("DefaultAlbumImportingPostTypeId", vm.DefaultAlbumImportingPostTypeId);
            _options.Set("DefaultVideoImportingPostTypeId", vm.DefaultVideoImportingPostTypeId);
            _options.Set("DefaultPostTypeId", vm.DefaultPostTypeId);
            _options.Set("Color1", vm.Color1);
            _options.Set("Color2", vm.Color2);
            _options.Set("SiteLanguage", vm.SiteLanguage);
            vm.PostTypeSelect = new SelectList(_managers.PostTypeManager.GetAll(), "Id", "SingularName");
            return View(vm);
        }
    }
}