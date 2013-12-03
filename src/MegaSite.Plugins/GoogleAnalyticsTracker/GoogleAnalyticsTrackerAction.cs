using System.Collections.Generic;
using System.Web;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Trash;

namespace MegaSite.Plugins.GoogleAnalyticsTracker
{
    public class GoogleAnalyticsTrackerAction : IActionPlugin
    {
        public object RunAction(string actionName, HttpContextBase context, List<Field> fields, IUnitOfWork data, IOptions pluginOptions)
        {
            return null;
        }

        public HtmlString OnFooter(IUnitOfWork data, IOptions pluginOptions)
        {
            var tracker = pluginOptions.Get("GoogleAnalyticsTracker");

            if (!string.IsNullOrEmpty(tracker))
            {
                return new HtmlString(@"<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');
  ga('create', '" + tracker + "', '" + UrlUtil.Domain + @"');
  ga('send', 'pageview');
</script>");
            }
            return new HtmlString("");
        }
    }
}
