using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Meganium.Api.Managers;

namespace Meganium.Site.Areas.Site.Controllers
{
    public class ThumbController : Controller
    {
        private readonly IManagers _managers;

        public ThumbController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Create(string fileName, string url)
        {
            var match = Regex.Match(fileName, @"^(.*)-(\d+)x(\d+)(-crop)*.jpg$");
            var name = match.Groups[1].Value;
            var width = Convert.ToInt32(match.Groups[2].Value);
            var height = Convert.ToInt32(match.Groups[3].Value);
            var crop = match.Groups[4].Value == "-crop";

            var thumbFilePath = _managers.MediaFileManager.GetThumb(url, name, width, height, crop);
            if (thumbFilePath != null)
            {
                return File(thumbFilePath, "image/jpeg");
            }
            return new HttpNotFoundResult();
        }
    }
}
