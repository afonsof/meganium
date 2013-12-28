using System.Collections.Generic;
using MegaSite.Api;

namespace MegaSite.Site.Areas.Extension.Models
{
    class PhotoSelectorData
    {
        public List<MediaFile> AvailableMediaFiles { get; set; }
        public List<MediaFile> SelectedMediaFiles { get; set; }
        public int PhotoCount { get; set; }
    }
}