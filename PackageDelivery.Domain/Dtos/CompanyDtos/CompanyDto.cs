﻿using System.ComponentModel.DataAnnotations;

namespace PackageDelivery.Domain.Dtos.CompanyDtos {
    public class CompanyDto {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int CurrencyId { get; set; }
        public double KilometerPrice { get; set; }
        public double KilogramPrice { get; set; }
        public double CubePrice { get; set; }
        [Display(Name = "Ratio")]
        public double VolumeAndWeightRatioToApplyVolumePrice { get; set; }
    }
}
