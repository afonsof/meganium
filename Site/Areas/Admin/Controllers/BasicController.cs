using System.Web.Mvc;
using Meganium.Api.Managers;
using Meganium.Api.ViewModels;
using Meganium.Api.Web;
using Newtonsoft.Json.Linq;

namespace Meganium.Site.Areas.Admin.Controllers
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

            var options = _managers.License.Options;
            var vm = new BasicIndexVm
            {
                FacebookId = options.GetLong("FacebookId"),
                GoogleAnalyticsTracker = options.GetString("GoogleAnalyticsTracker"),
                SiteTitle = options.GetString("SiteTitle"),
                AdminEmail = options.GetString("AdminEmail"),
                Theme = options.GetString("Theme"),
                SiteDescription = options.GetString("SiteDescription"),
                SiteLanguage = options.GetString("SiteLanguage"),
                DefaultAlbumImportingPostTypeId = options.GetInt("DefaultAlbumImportingPostTypeId"),
                DefaultVideoImportingPostTypeId = options.GetInt("DefaultVideoImportingPostTypeId"),
                DefaultPostTypeId = options.GetInt("DefaultPostTypeId"),
                Color1 = options.GetString("Color1"),
                Color2 = options.GetString("Color2"),
                PostTypeSelect = new SelectList(_managers.PostTypeManager.GetAll(), "Id", "SingularName"),
                AllowImportMediaFiles = options.Get("AllowImportMediaFiles", false),
                DataJson = _managers.License.OptionsJson
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(BasicIndexVm vm)
        {
            var options = _managers.License.Options;

            var data = JObject.Parse(vm.DataJson);
            foreach (var item in data)
            {
                options.Set(item.Key, item.Value);    
            }
            
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