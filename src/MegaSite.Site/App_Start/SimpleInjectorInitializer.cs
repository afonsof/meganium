using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(MegaSite.Site.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace MegaSite.Site.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcAttributeFilterProvider();
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            var webLifestyle = new WebRequestLifestyle();
            container.Register(() => Options.Instance);
            container.Register(() => CreateManagers(container), webLifestyle);
        }

        private static IManagers CreateManagers(Container container)
        {
            IManagers managers = new UnitOfWork(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            return managers;
            
            //todo:
            //var pluginComposite = new ActionPluginComposite(d);
            /*var allTypes = Assembly.GetAssembly(typeof(EmailFormAction)).GetTypes();
            var allPlugins = allTypes.Where(p => typeof(IActionPlugin).IsAssignableFrom(p));
            var plugins =
                allPlugins.Select(
                    p => Activator.CreateInstance(p) as IActionPlugin);

            pluginComposite.AddRange(plugins);
            d.Plugin = pluginComposite;*/
        }
    }
}