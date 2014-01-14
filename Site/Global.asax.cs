using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;
using Meganium.Api.Managers;
using Meganium.Api.Trash;

namespace Meganium.Site
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
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SimpleInjectorInitializer.Initialize();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
#if RELEASE
            var exc = Server.GetLastError();
            Mailer.SendToAdmin("Meganium Site Error", "error@meganium.com.br", "Alerta de erro Meganium", Request.Url + "\n" + exc.Message + "\n" + exc.StackTrace);
            Server.Transfer("~/Site/Home/Error");
            Server.ClearError();
#endif
        }
    }
}
