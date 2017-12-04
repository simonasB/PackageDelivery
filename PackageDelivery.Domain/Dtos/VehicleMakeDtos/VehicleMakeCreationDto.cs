using System.ComponentModel.DataAnnotations;

namespace PackageDelivery.Domain.Dtos.VehicleMakeDtos {
    public class VehicleMakeCreationDto {
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
