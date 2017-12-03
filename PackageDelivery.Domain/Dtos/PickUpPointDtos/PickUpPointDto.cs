namespace PackageDelivery.Domain.Dtos.PickUpPointDtos {
    public class PickUpPointDto {
        public int PickUpPointId { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
