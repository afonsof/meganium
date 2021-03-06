﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Meganium.Api.Entities;
using Meganium.Api.Resources;

namespace Meganium.Api.ViewModels
{
    public class ClientEditVm: IHaveDataJson
    {
        public int Id { get; set; }

        [Required, StringLength(64)]
        [Display(Name = "FullName", ResourceType = typeof(Resource))]
        public string FullName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Display(Name="Enabled", ResourceType = typeof(Resource))]
        public bool Enabled { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resource))]
        public string Code { get; set; }

        public string Memo { get; set; }

        public string AvailableMediaFilesJson { get; set; }
        public IEnumerable<MediaFile> SelectedMediaFiles { get; set; }

        [Display(Name = "PhotoCount", ResourceType = typeof(Resource))]
        public int PhotoCount { get; set; }

        public string DataJson { get; set; }
    }
}