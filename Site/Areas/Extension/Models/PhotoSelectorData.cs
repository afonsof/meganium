using System.Collections.Generic;
using Meganium.Api;

namespace Meganium.Site.Areas.Extension.Models
{
    class PhotoSelectorData
    {
        public List<MediaFile> AvailableMediaFiles { get; set; }
        public List<MediaFile> SelectedMediaFiles { get; set; }
        public int PhotoCount { get; set; }
    }
}