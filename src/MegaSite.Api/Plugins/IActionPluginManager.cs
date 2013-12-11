using System.Web;
using System.Web.Mvc;

namespace MegaSite.Api.Plugins
{
    public interface IActionPluginManager
    {
        HtmlString OnFooter();
        HtmlString RunAction(string pluginName, string actionName, HttpContextBase context);
    }
}