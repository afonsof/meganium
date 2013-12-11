using System.Web;

namespace MegaSite.Api.Plugins
{
    public interface IActionPlugin
    {
        HtmlString RunAction(string actionName, HttpContextBase context, IManagers managers);
        HtmlString OnFooter(IManagers managers);
    }
}
