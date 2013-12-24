using System;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Managers;
using Newtonsoft.Json.Linq;

namespace MegaSite.Site.Areas.Extension.Controllers
{
    public class RsvpOptions
    {
        public DateTime OpenDate { get; set; }
        public DateTime Deadline { get; set; }
        public string GreetingMessage { get; set; }
        public DateTime YesVerbiage { get; set; }
        public DateTime NoVerbiage { get; set; }
        public DateTime ThankYouMessage { get; set; }
    }

    public class RsvpGuest
    {
        
    }

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

        public ActionResult Data(string hash)
        {
            var client = _managers.ClientManager.GetHavingHashInObject(hash);

            var jobject = JObject.Parse(client.DataJson);

            return View();
        }

    }
}
