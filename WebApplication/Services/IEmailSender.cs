using System.Threading.Tasks;

namespace PackageDelivery.WebApplication.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
