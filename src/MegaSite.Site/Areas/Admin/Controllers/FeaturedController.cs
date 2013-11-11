using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MegaSite.Api;
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
            var allPosts = _managers.PostManager
                .GetWhatAllowsMarkAsFeatured()
                .ToList();

            var featuredPosts = _managers.PostManager
                .GetFeatured()
                .Select(bp => bp.Id)
                .ToList();

            return View(new MultiSelectList(allPosts, "Id", "TitleWithType", featuredPosts));
        }

        [HttpPost]
        public ActionResult Manage(List<int> featuredPosts)
        {
            _managers.PostManager.SetFeatureds(featuredPosts);
            return RedirectToAction("Index");
        }
    }
}