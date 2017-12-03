using AutoMapper;
using PackageDelivery.Domain.Dtos.CompanyDtos;
using PackageDelivery.Domain.Dtos.PickUpPointDtos;
using PackageDelivery.Domain.Dtos.VehicleDtos;
using PackageDelivery.Domain.Dtos.VehicleMakeDtos;
using PackageDelivery.Domain.Dtos.VehicleModelDtos;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Models.Api {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyCreationDto, Company>();
            CreateMap<CompanyUpdateDto, Company>();

            CreateMap<VehicleMake, VehicleMakeDto>();
            CreateMap<VehicleMakeCreationDto, VehicleMake>();
            CreateMap<VehicleMakeUpdateDto, VehicleMake>();

            CreateMap<VehicleModel, VehicleModelDto>();
            CreateMap<VehicleModelCreationDto, VehicleModel>();
            CreateMap<VehicleModelUpdateDto, VehicleModel>();

            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleCreationDto, Vehicle>();
            CreateMap<VehicleUpdateDto, Vehicle>();

            CreateMap<PickUpPoint, PickUpPointDto>();
            CreateMap<PickUpPointCreationDto, PickUpPoint>();
            CreateMap<PickUpPointUpdateDto, PickUpPoint>();
        }
    }
}
