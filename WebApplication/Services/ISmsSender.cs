using System.Threading.Tasks;

namespace PackageDelivery.WebApplication.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
