using System.Web;
using MegaSite.Api.Managers;

namespace MegaSite.Api.Plugins
{
    public interface IActionPlugin
    {
        HtmlString RunAction(string actionName, HttpContextBase context, IManagers managers);
        HtmlString OnFooter(IManagers managers);
    }
}
