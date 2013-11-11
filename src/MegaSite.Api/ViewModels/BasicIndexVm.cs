using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NHibernate.Validator.Constraints;

namespace MegaSite.Api.ViewModels
{
    public class BasicIndexVm
    {
        public long? FacebookId { get; set; }
        public string GoogleAnalyticsTracker { get; set; }

        [Required]
        public string SiteTitle { get; set; }

        [Required, Email]
        public string AdminEmail { get; set; }

        [Required]
        public string Theme { get; set; }

        public string SiteDescription { get; set; }

        [Required]
        public int? DefaultAlbumImportingPostTypeId { get; set; }

        [Required]
        public int? DefaultVideoImportingPostTypeId { get; set; }

        [Required]
        public string Color1 { get; set; }

        [Required]
        public string Color2 { get; set; }

        public SelectList PostTypeSelect { get; set; }

        [Required]
        public string SiteLanguage { get; set; }

        public int DefaultPostTypeId { get; set; }
    }
}