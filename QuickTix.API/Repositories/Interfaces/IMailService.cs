using QuickTix.API.Entities.Services;

namespace QuickTix.API.Repositories.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestService mailRequest);
    }
}
