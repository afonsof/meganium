using System.Text;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Dongle.System;
using MegaSite.Api;
using MegaSite.Api.Managers;
using MegaSite.Api.Trash;
using MegaSite.Api.Web;
using MegaSite.Api.Resources;

namespace MegaSite.Site.Areas.Site.Controllers
{
    public class RssController : BaseController
    {
        private readonly IManagers _managers;

        public RssController(IManagers managers)
        {
            _managers = managers;
        }

        [DonutOutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            var lastItem = _managers.PostManager.GetLastPublished();
            if (lastItem == null)
            {
                return new HttpNotFoundResult();
            }

            var absoluteUrl = PathResolver.AbsoluteUrl(Request.Url, "~/");
            var items = _managers.PostManager.GetLastPublished(10);
            var options = _managers.License.Options;

            var response = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>")
            .Append("<rss version=\"2.0\" xmlns:content=\"http://purl.org/rss/1.0/modules/content/\" xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:atom=\"http://www.w3.org/2005/Atom\">")
            .Append("<channel><title>").Append(options.Get("SiteTitle")).Append("</title>")
            .Append("<atom:link href=\"").Append(absoluteUrl).Append("\" rel=\"self\" type=\"application/rss+xml\" />")
            .Append("<link>").Append(absoluteUrl).Append("</link>")
            .Append("<description>").Append(options.Get("SiteDescription")).Append("</description>")
            .Append("<lastBuildDate>").Append(lastItem.PublishedAt.Value.ToRssTime()).Append("</lastBuildDate>")
            .Append("<language>").Append(options.Get("SiteLanguage")).Append("</language>")
            .Append("<generator>").Append(Resource.ProjectName).Append("</generator>");
            
            foreach (var item in items)
            {
                var creator = item.CreatedBy != null ? item.CreatedBy.DisplayName : Resource.Anonymous;
                response.Append("<item>")
                .Append("<title>").Append(item.Title).Append("</title>")
                .Append("<link>").Append(PathResolver.AbsoluteUrl(Request.Url, item.UrlPath)).Append("</link>")
                .Append("<pubDate>").Append(item.PublishedAt.Value.ToRssTime()).Append("</pubDate>")
                .Append("<dc:creator>").Append(creator).Append("</dc:creator>")
                .Append("<description><![CDATA[").Append(item.PreviewContent).Append("]]></description>")
                .Append("<content:encoded><![CDATA[").Append(item.Content).Append("]]></content:encoded>")
                .Append("</item>");
            }
            response.Append("</channel></rss>");

            Response.ContentType = "application/xml";
            Response.Write(response);
            return null;
        }
    }
}