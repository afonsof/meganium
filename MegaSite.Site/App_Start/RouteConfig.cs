using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using Dongle.System;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;

namespace MegaSite.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            try
            {
                using (var db = new Database(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    var repo = new Repository<PostType>(db);
                    foreach (var postType in repo.AsQueryable())
                    {
                        if (string.IsNullOrEmpty(postType.SingularName) || string.IsNullOrEmpty(postType.PluralName))
                        {
                            continue;
                        }
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
                new {controller = "Thumb", action = "Create"},
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