using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using OutsourcePlatformApp.Controllers;

namespace OutsourcePlatformApp.Service.EmailServices;

public class SendVerificationEmail : IEmailSenderService
{
    private const string mailPassword = "jD7tJcfJ756tLUHpbWtv";
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration configuration;

    public SendVerificationEmail(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.configuration = configuration;
    }

    public async Task<bool> SendEmail(string userEmail, string message)
    {
        var hostname = httpContextAccessor.HttpContext.Request.Host;
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Администрация сайта", "ateplinsky@mail.ru"));
        emailMessage.Subject = "Подтверждение почты";
        emailMessage.To.Add(new MailboxAddress("", userEmail));
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = $"<a href=http://localhost:3000/verification?code={message}> Подтвердить почту</a>"
        };
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.mail.ru", 465, true);
            await client.AuthenticateAsync("ateplinsky@mail.ru", mailPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        return true;
    }
}