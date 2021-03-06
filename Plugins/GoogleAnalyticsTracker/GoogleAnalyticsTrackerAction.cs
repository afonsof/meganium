﻿using System.Web;
using Meganium.Api.Managers;
using Meganium.Api.Plugins;
using Meganium.Api.Trash;

namespace Meganium.Plugins.GoogleAnalyticsTracker
{
    public class GoogleAnalyticsTrackerAction : IActionPlugin
    {
        public HtmlString RunAction(string actionName, HttpContextBase context, IManagers managers)
        {
            return null;
        }

        public HtmlString OnFooter(IManagers managers)
        {
            var tracker = managers.License.Options.GetString("GoogleAnalyticsTracker");

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
