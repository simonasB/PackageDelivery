using System.ComponentModel.DataAnnotations;

namespace PackageDelivery.WebApplication.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
