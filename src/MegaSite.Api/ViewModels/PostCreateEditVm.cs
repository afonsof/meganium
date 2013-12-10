using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MegaSite.Api.Entities;
using MegaSite.Api.Resources;

namespace MegaSite.Api.ViewModels
{
    public class PostCreateEditVm
    {
        public List<Field> Fields { get; set; }

        public MultiSelectList CategoriesMultiselect { get; set; }

        public SelectList PrivacySelect { get; set; }

        public SelectList ParentSelect { get; set; }

        public PostType PostType { get; set; }

        public int Id { get; set; }

        [Required]
        [Display(Name="Title", ResourceType = typeof(Resource))]
        public string Title { get; set; }

        public DateTime PublishedAt { get; set; }
        public bool Published { get; set; }
        public string Content { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FeaturedMediaFileJson { get; set; }
        public string MediaFilesJson { get; set; }
        public string Hash { get; set; }
    }
}