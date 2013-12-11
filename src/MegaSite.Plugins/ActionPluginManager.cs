using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Plugins;

namespace MegaSite.Plugins
{
    public class ActionPluginManager : IActionPluginManager
    {
        private readonly IManagers _managers;

        public ActionPluginManager(IManagers managers)
        {
            _managers = managers;
        }

        public HtmlString OnFooter()
        {
            var html = new StringBuilder();
            foreach (var pluginType in Plugins)
            {
                var plugin = Activator.CreateInstance(pluginType) as IActionPlugin;
                if (plugin != null)
                {
                    html.Append(plugin.OnFooter(_managers));
                }
            }
            return new HtmlString(html.ToString());
        }
        public HtmlString RunAction(string pluginName, string actionName, HttpContextBase context)
        {
            var pluginType = Plugins.FirstOrDefault(p => p.Name.ToUpperInvariant() == pluginName.ToUpperInvariant());

            if (pluginType != null)
            {
                var plugin = Activator.CreateInstance(pluginType) as IActionPlugin;
                if (plugin != null)
                {
                    return plugin.RunAction(actionName.ToLowerInvariant(), context, _managers);
                }
            }
            return null;
        }

        private static IEnumerable<Type> Plugins
        {
            get
            {
                var items = Assembly.GetAssembly(typeof (ContactForm.ContactForm))
                    .GetTypes()
                    .Where(p => typeof (IActionPlugin).IsAssignableFrom(p));
                return items;
            }
        }
    }
}