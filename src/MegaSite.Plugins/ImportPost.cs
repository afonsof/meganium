using System;

namespace MegaSite.Plugins
{
    public class ImportPost
    {
        public string Title { get; set; }
        public string CreatedByStr { get; set; }
        public string PostTypeStr { get; set; }
        public string ParentStr { get; set; }
        public string CategoriesStr { get; set; }
        public string ExternalServiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool Published { get; set; }
        public string Location { get; set; }
        public string ExternalServiceUser { get; set; }
        public string MediaFilesJson { get; set; }
        public string FeaturedMediaFileJson { get; set; }
        public string Content { get; set; }
        public int Id { get; set; }
        public uint MediaFilesCount { get; set; }
    }
}