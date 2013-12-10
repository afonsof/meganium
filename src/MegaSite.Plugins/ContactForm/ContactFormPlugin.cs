using System.Collections.Generic;
using System.Linq;
using System.Web;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Plugins;
using MegaSite.Api.Resources;
using MegaSite.Api.Tools;

namespace MegaSite.Plugins.ContactForm
{
    public class ContactFormPlugin : IActionPlugin
    {
        public object RunAction(string actionName, HttpContextBase context, List<Field> fields, IUnitOfWork data, IOptions pluginOptions)
        {
            if (actionName == "send")
            {
                var body = "De: " + context.Request["name"] + " (" + context.Request["email"] + ")\n";
                foreach (var key in context.Request.Form.AllKeys.Where(k => k != "Name" && k != "Email"))
                {
                    var name = Resource.ResourceManager.GetString(key) ?? key;
                    body += name + ": " + context.Request.Form[key] + "\n";
                }

                Mailer.Send(context.Request["Name"], context.Request["Email"], "Contato do Site " + Options.Instance.Get("SiteTitle"), body);
                context.Response.Redirect("~/");
                //bug #23 : Não está mostrando  mensagem usuário e nem validação no Plugin de EmailSender
            }
            return null;
        }

        public HtmlString OnFooter(IUnitOfWork uow, IOptions pluginOptions)
        {
            return null;
        }
    }
}