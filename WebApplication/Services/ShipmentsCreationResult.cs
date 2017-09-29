namespace PackageDelivery.WebApplication.Services
{
    public interface IShipmentsCreationError {
        string GetResult();
    }

    public class NoOrdersAvailableError : IShipmentsCreationError {
        public string GetResult() {
            return "No orders available.";
        }
    }

    public class NoVehiclesAvailableError : IShipmentsCreationError
    {
        public string GetResult()
        {
            return "No vehicles available.";
        }
    }

    public class NoDriversAvailableError : IShipmentsCreationError
    {
        public string GetResult()
        {
            return "No drivers available.";
        }
    }

    public class NullError : IShipmentsCreationError
    {
        public string GetResult()
        {
            return "";
        }
    }
}
