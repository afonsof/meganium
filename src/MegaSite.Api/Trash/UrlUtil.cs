using System;
using System.Web;

namespace MegaSite.Api.Trash
{
    public static class UrlUtil
    {
        public static string UrlRoot
        {
            get
            {
                var url = HttpContext.Current.Request.Url;
                var newUrl = string.Format("{0}{1}{2}", url.Scheme, Uri.SchemeDelimiter, url.Authority);
                return newUrl;
            }
        }

        public static string Domain
        {
            get
            {
                return HttpContext.Current.Request.Url.Authority;
            }
        }

        public static string VirtualPath
        {
            get
            {
                var path = HttpRuntime.AppDomainAppVirtualPath;
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }
                if (!path.EndsWith("/"))
                {
                    path += "/";
                }


                return path;
            }
        }

        public static string ApplicationFolder
        {
            get
            {
                var segments = HttpContext.Current.Request.Url.Segments;
                var newUrl = "";

                for (var x = 0; x < segments.Length - 1; x++)
                {
                    newUrl += segments[x];
                }

                var virtualPath = VirtualPath;
                if (newUrl.StartsWith(virtualPath))
                {
                    newUrl = newUrl.Substring(virtualPath.Length);
                }
                return newUrl;
            }
        }

        public static string RootApplicationUrl
        {
            get
            {
                return UrlRoot + VirtualPath;
            }
        }
    }
}