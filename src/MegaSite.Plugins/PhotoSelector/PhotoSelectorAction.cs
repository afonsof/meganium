using System;
using System.Collections.Generic;
using System.Web;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Plugins;
using MegaSite.Api.Tools;

namespace MegaSite.Plugins.PhotoSelector
{
    public class PhotoSelectorAction : IActionPlugin
    {
        public object RunAction(string actionName, HttpContextBase context, List<Field> fields, IUnitOfWork data, IOptions pluginOptions)
        {
            if (actionName == "send")
            {
                var post = data.PostRepository.GetById(Convert.ToInt32(context.Request.Form["postId"]));

                if (post != null)
                {
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

        public HtmlString OnFooter(IUnitOfWork data, IOptions pluginOptions)
        {
            return null;
        }
    }
}