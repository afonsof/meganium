using System;
using System.Web;

namespace MegaSite.Api.Trash
{
    public class PathResolver
    {
        private string _root;
        private readonly IOptions _options;

        public PathResolver(IOptions options)
        {
            _options = options;
        }

        public string Themes
        {
            get
            {
                return "~/Content/themes/";
            }
        }

        public string Licenses
        {
            get
            {
                return "~/Content/licenses/";
            }
        }

        public string CurrentTheme
        {
            get
            {
                return _root ?? (_root = string.Format("~/Content/themes/{0}/", _options.Get("theme")));
            }
        }

        public string CurrentThemeViews
        {
            get
            {
                return string.Format("{0}Views/", CurrentTheme);
            }
        }

        public static string AbsoluteUrl(Uri url, string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            if (url == null)
            {
                throw new Exception("URL can't be null");
            }
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}