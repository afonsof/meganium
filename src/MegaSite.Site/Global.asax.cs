using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;
using MegaSite.Api.Managers;
using MegaSite.Api.Tools;
using MegaSite.Api.Trash;
using MegaSite.Site.App_Start;

namespace MegaSite.Site
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!MediaFileManager.SetupWasCalled())
            {
                MediaFileManager
                    .Setup(HttpContext.Current.Server.MapPath("~/"), "Content/Uploads/Files", "Content/Uploads/Thumbs",
                    PathResolver.AbsoluteUrl(HttpContext.Current.Request.Url, "~/"));
            }

#if DEBUG
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
#endif
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SimpleInjectorInitializer.Initialize();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exc = Server.GetLastError();
            Mailer.Send("MegaSite", "Sistema@gmail.com", "Alerta de erro MegaSite", exc.Message);
            Server.Transfer("~/Site/Home/Error");
            Server.ClearError();
        }
    }
}
