using System.Web.Mvc;
using MegaSite.Api.Messaging;

namespace MegaSite.Api.Web
{
    public class BaseController : Controller
    {
        protected ActionResult RedirectToAction(string action, string controller, object routeValues, string message, MessageType messageType = MessageType.Info)
        {
            SetMessage(message, messageType);
            return RedirectToAction(action, controller, routeValues);
        }

        protected ActionResult RedirectToAction(string action, string message, MessageType messageType = MessageType.Info)
        {
            SetMessage(message, messageType);
            return RedirectToAction(action);
        }

        protected void SetMessage(string text, MessageType type)
        {
            TempData["Message"] = new Message(text, type);
        }

        protected void SetMessage(Message message)
        {
            TempData["Message"] = message;
        }

        protected ActionResult RedirectToAction(string action, Message message)
        {
            SetMessage(message);
            return RedirectToAction(action);
        }

    }
}