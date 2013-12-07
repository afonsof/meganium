using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Messaging;
using MegaSite.Api.Resources;
using MegaSite.Api.Web;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class FeaturedController : BaseController
    {
        private readonly IManagers _managers;

        public FeaturedController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View(_managers.PostManager.GetFeatured());
        }

        public ActionResult Manage()
        {
            var whatAllowsMarkAsFeatured = _managers.PostManager.GetWhatAllowsMarkAsFeatured();
            var allPosts = whatAllowsMarkAsFeatured
                .ToList();

            var featuredPosts = _managers.PostManager
                .GetFeatured()
                .Select(bp => bp.Id)
                .ToList();

            return View(new MultiSelectList(allPosts, "Id", "TitleWithType", featuredPosts));
        }

        [HttpPost]
        public ActionResult Manage(List<int> featureds)
        {
            _managers.PostManager.SetFeatureds(featureds);
            return RedirectToAction("Index", Resource.ItemSuccessfullySaved, MessageType.Success);
        }
    }
}