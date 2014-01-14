using System.Web;
using Meganium.Api.Managers;

namespace Meganium.Api.Plugins
{
    public interface IActionPlugin
    {
        HtmlString RunAction(string actionName, HttpContextBase context, IManagers managers);
        HtmlString OnFooter(IManagers managers);
    }
}
