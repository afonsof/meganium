using System.Web.Mvc;

namespace MegaSite.Site.Areas.Site
{
    public class SiteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Site";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Site_default",
                "Site/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
