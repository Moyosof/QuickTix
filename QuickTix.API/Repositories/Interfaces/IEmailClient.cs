using QuickTix.API.Entities.Services;

namespace QuickTix.API.Repositories.Interfaces
{
    public interface IEmailClient
    {
        Task SendEmailAsync(MailRequestService mailRequest);
    }
}
