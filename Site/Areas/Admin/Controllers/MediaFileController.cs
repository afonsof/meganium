using System.Web;
using System.Web.Mvc;
using Meganium.Api.Managers;
using Meganium.Api.Trash;
using Meganium.Api.Web;

namespace Meganium.Site.Areas.Admin.Controllers
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