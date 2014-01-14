using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MegaSite.Api.Resources;

namespace MegaSite.Api.ViewModels
{
    public class UserEditVm
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
    }
}