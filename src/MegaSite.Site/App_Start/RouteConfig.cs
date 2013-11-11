using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Dongle.System;
using MegaSite.Api;

namespace MegaSite.Site.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );*/

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*routes.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {controller = "Post", action = "Index", id = UrlParameter.Optional},
                new[] { "MegaSite.Site.Areas.Admin.Controllers" });*/

            try
            {
                using (var data = new UnitOfWork(null, ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    foreach (var postType in data.PostTypeManager.GetAll())
                    {
                        var singular = postType.SingularName.ToSlug();
                        var plural = postType.PluralName.ToSlug();

                        routes.MapRoute(
                            singular + "RouteShow",
                            singular + "/{slug}",
                            new {controller = "Home", action = "SlugShow", postTypeId = postType.Id},
                            new[] { "MegaSite.Site.Areas.Site.Controllers" }
                            );

                        routes.MapRoute(
                            singular + "RouteCategory",
                            plural + "/{slug}",
                            new {controller = "Home", action = "SlugCategory", postTypeId = postType.Id},
                            new[] { "MegaSite.Site.Areas.Site.Controllers" }
                            );

                        routes.MapRoute(
                            singular + "RouteIndex",
                            plural,
                            new {controller = "Home", action = "SlugIndex", postTypeId = postType.Id},
                            new[] { "MegaSite.Site.Areas.Site.Controllers" }
                            );
                    }
                }
            }
            catch
            {

            }
            routes.MapRoute(
                "PluginAction",
                "plugin/{pluginName}/{pluginAction}",
                new {controller = "Home", action = "SlugPluginAction"},
                new[] { "MegaSite.Site.Areas.Site.Controllers" }
                );

            routes.MapRoute(
                "Thumb",
                "content/uploads/thumbs/{fileName}",
                new {controller = "Thumb", action = "Index"},
                new[] { "MegaSite.Site.Areas.Site.Controllers" }
                );

            routes.MapRoute(
                "Rss",
                "Rss",
                new {controller = "Rss", action = "Index"},
                new[] { "MegaSite.Site.Areas.Site.Controllers" }
                );


            routes.MapRoute("Default",
                "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                new[] { "MegaSite.Site.Areas.Site.Controllers" });
        }
    }
}