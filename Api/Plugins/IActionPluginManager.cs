using System.Web;

namespace Meganium.Api.Plugins
{
    public interface IActionPluginManager
    {
        HtmlString OnFooter();
        HtmlString RunAction(string pluginName, string actionName, HttpContextBase context);
    }
}