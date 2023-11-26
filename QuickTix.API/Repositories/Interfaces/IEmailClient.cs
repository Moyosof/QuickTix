using HouseMate.API.Entities.Services;

namespace HouseMate.API.Repositories.Interfaces
{
    public interface IEmailClient
    {
        Task SendEmailAsync(MailRequestService mailRequest);
    }
}
