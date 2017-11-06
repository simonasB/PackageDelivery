using AutoMapper;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Models.Api {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Company, CompanyModel>();
            CreateMap<CompanyModel, Company>();

            CreateMap<VehicleMake, VehicleMakeModel>();
            CreateMap<VehicleMakeModel, VehicleMake>();

            CreateMap<VehicleModel, VehicleModelViewModel>();
            CreateMap<VehicleModelViewModel, VehicleModel>();
        }
    }
}
