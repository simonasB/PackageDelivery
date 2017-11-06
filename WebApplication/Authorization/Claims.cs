using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Authorization
{
    public static class Claims {
        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string Role = "role";
        public const string CompanyId = nameof(Company.CompanyId);
        public const string PickUpPointId = nameof(PickUpPoint.PickUpPointId);
        public const string Email = "email";
    }
}
