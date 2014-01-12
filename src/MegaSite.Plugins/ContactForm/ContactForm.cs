using System.Linq;
using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Managers;
using MegaSite.Api.Plugins;
using MegaSite.Api.Resources;
using MegaSite.Api.Tools;

namespace MegaSite.Plugins.ContactForm
{
    public class ContactForm : IActionPlugin
    {
        public HtmlString RunAction(string actionName, HttpContextBase context, IManagers managers)
        {
            if (actionName == "sendmail")
            {
                var body = "De: " + context.Request["name"] + " (" + context.Request["email"] + ")\n";
                foreach (var key in context.Request.Form.AllKeys.Where(k => k != "Name" && k != "Email"))
                {
                    var name = Resource.ResourceManager.GetString(key) ?? key;
                    body += name + ": " + context.Request.Form[key] + "\n";
                }
                // TODO: Remover strings
                var options = managers.License.Options;
                Mailer.Send(context.Request["Name"], context.Request["Email"], "Contato do Site " + options.Get("SiteTitle"), body, options.Get("AdminEmail"));
                context.Response.Redirect("~/");
            }
            return null;
        }

        public HtmlString OnFooter(IManagers managers)
        {
            return null;
        }
    }
}