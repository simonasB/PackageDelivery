namespace PackageDelivery.Domain.Dtos.CompanyDtos {
    public class CompanyUpdateDto {
        public string Name { get; set; }
        public string Email { get; set; }
        public double KilometerPrice { get; set; }
        public double KilogramPrice { get; set; }
        public double CubePrice { get; set; }
        public double VolumeAndWeightRatioToApplyVolumePrice { get; set; }
    }
}
