﻿using System.ComponentModel.DataAnnotations;

namespace PackageDelivery.WebApplication.Models.AccountViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
        public bool IsDriver { get; set; }
        public int CompanyId { get; set; }
    }
}