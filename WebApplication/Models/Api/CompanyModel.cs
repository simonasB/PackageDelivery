namespace PackageDelivery.WebApplication.Models.Api
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int CurrencyId { get; set; }
        public double KilometerPrice { get; set; }
        public double KilogramPrice { get; set; }
        public double CubePrice { get; set; }
        public double VolumeAndWeightRatioToApplyVolumePrice { get; set; }
    }
}
