using System.Collections.Generic;
using System.Web;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Plugins
{
    public interface IActionPlugin
    {
        object RunAction(string actionName, HttpContextBase context, List<Field> fields, IUnitOfWork uow, IOptions pluginOptions);
        HtmlString OnFooter(IUnitOfWork uow, IOptions pluginOptions);
    }
}
