using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MegaSite.Api.Resources;

namespace MegaSite.Api.ViewModels
{
    public class UserCreateVm : UserEditVm
    {
        [Required, StringLength(255)]
        [Display(Name = "InitalPassword", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public new string UserName { get; set; }

        [Required, Email, StringLength(128)]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public new string Email { get; set; }
    }
}