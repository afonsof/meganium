using System.Linq;
using System.Web.Mvc;
using MegaSite.Api;

namespace MegaSite.Site.Areas.Extension.Controllers
{
    public class PhotoSelectorController : Controller
    {
        private readonly IManagers _managers;

        public PhotoSelectorController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data(string id)
        {
            var post = _managers.PostManager.GetByHash(id);

            if (post == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(post.MediaFiles.Select(mf=>new { src = mf.Url}), JsonRequestBehavior.AllowGet);
        }
    }
}
