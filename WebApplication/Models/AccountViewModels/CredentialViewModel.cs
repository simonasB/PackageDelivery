using System.ComponentModel.DataAnnotations;

namespace PackageDelivery.WebApplication.Models.AccountViewModels {
    public class CredentialViewModel {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
