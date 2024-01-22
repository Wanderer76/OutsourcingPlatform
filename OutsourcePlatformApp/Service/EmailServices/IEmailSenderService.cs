
namespace OutsourcePlatformApp.Service.EmailServices;

public interface IEmailSenderService
{
    Task<bool> SendEmail(string userEmail, string message);
}