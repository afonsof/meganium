using System;
using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Plugins;
using MegaSite.Api.Tools;

namespace MegaSite.Plugins.PhotoSelector
{
    public class PhotoSelectorAction : IActionPlugin
    {
        public HtmlString RunAction(string actionName, HttpContextBase context, IManagers managers)
        {
            if (actionName == "send")
            {
                var post = managers.PostManager.GetById(Convert.ToInt32(context.Request.Form["postId"]));

                if (post != null)
                {
                    // TODO: Remover strings
                    var photos = context.Request.Form["titles"];

                    var body = "Cliente: " + post.Title + "\n";
                    body += "Limite de fotos: " + post.FieldsValues["Limite de fotos"] + "\n";
                    body += "Quantidade de fotos escolhidas: " + photos.Split('\n').Length + "\n";
                    body += "\nFotos:\n\n" + photos;

                    Mailer.Send(post.Title, "", "Seleção de fotos do cliente " + post.Title, body);
                    context.Response.Redirect("~/");
                }

            }
            return null;
        }

        public HtmlString OnFooter(IManagers managers)
        {
            return null;
        }
    }
}