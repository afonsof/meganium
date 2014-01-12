using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Plugins;
using MegaSite.Plugins;
using MegaSite.Site;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SimpleInjectorInitializer), "Initialize")]

namespace MegaSite.Site
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