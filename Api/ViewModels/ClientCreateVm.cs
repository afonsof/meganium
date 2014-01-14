using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Meganium.Api.Resources;

namespace Meganium.Api.ViewModels
{
    public class ClientCreateVm : ClientEditVm
    {
        [Required, Email, StringLength(128)]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public new string Email { get; set; }

        public string Memo { get; set; }
    }
}