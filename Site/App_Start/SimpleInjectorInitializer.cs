using System.Reflection;
using System.Web.Mvc;
using Meganium.Api;
using Meganium.Api.Managers;
using Meganium.Api.Plugins;
using Meganium.Plugins;
using Meganium.Site;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SimpleInjectorInitializer), "Initialize")]

namespace Meganium.Site
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
            container.Register(CreateManagers, webLifestyle);
            container.Register<IActionPluginManager, ActionPluginManager>();
        }

        private static IManagers CreateManagers()
        {
            IManagers managers = new UnitOfWork();
            return managers;
        }
    }
}