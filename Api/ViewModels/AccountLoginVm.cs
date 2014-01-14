using System.ComponentModel.DataAnnotations;
using NHibernate.Validator.Constraints;

namespace Meganium.Api.ViewModels
{
    public class AccountLoginVm
    {
        [Required, Email]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}