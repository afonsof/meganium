using System.Net;
using System.Web.Mvc;
using MegaSite.Api.Managers;
using MegaSite.Api.Trash;
using MegaSite.Site.Areas.Extension.Models;

namespace MegaSite.Site.Areas.Extension.Controllers
{
    public class RsvpController : Controller
    {
        private IManagers _managers { get; set; }

        public RsvpController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInvitation(string code)
        {
            var guest = _managers.ClientSubItemManager.GetByCode(code);
            if (guest == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Este convidado não existe");
            }
            if (guest.GetData<RsvpData>().Confirm)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Este convidado já confirmou a presença");
            }
            return Content(guest.DataJson, "application/json");
        }

        [HttpPost]
        public ActionResult AcceptInvitation(string code, string name)
        {
            var guest = _managers.ClientSubItemManager.GetByCode(code);
            if (guest == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Este convidado não existe");
            }
            var rsvpData = guest.GetData<RsvpData>();
            if (string.IsNullOrEmpty(rsvpData.Name))
            {
                if (string.IsNullOrEmpty(name))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "É necessário digitar um nome para este convidado");
                }
                rsvpData.Name = name;
            }
            rsvpData.Confirm = true;
            guest.SetData(rsvpData);
            _managers.ClientSubItemManager.Change(guest);

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Presença confirmada com sucesso!");
        }
    }
}
