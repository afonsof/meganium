using System.Web.Mvc;

namespace MegaSite.Site.Areas.Extension
{
    public class ExtensionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Extension";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Extension_default",
                "Extension/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
