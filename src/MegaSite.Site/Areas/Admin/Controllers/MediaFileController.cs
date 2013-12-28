using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Managers;
using MegaSite.Api.Trash;
using MegaSite.Api.Web;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class MediaFileController : BaseController
    {
        private readonly IManagers _managers;

        public MediaFileController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Recents()
        {
            return Json(_managers.MediaFileManager.GetRecents(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase fileData)
        {
            if (fileData == null) return null;
            var mediaFile = _managers.MediaFileManager.Save(fileData);
            return Content(InternalJsonSerializer.Serialize(mediaFile));
        }
    }
}