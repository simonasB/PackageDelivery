using System.ComponentModel.DataAnnotations;

namespace PackageDelivery.Domain.Dtos.CompanyDtos {
    public class CompanyCreationDto {
        [Required]
        [MaxLength(255)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public double KilometerPrice { get; set; }
        [Required]
        public double KilogramPrice { get; set; }
        [Required]
        public double CubePrice { get; set; }
        [Required]
        [Display(Name = "Ratio")]
        public double VolumeAndWeightRatioToApplyVolumePrice { get; set; }
    }
}
