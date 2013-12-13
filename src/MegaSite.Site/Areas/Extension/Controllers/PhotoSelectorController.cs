using System.Web.Mvc;

namespace MegaSite.Site.Areas.Extension.Controllers
{
    public class PhotoSelectorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()
        {
            return Json(new[]
            {
                new
                {
                    test = 1
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
