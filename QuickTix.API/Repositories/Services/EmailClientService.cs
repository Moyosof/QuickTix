using QuickTix.API.Entities.Services;
using QuickTix.API.Repositories.Interfaces;

namespace QuickTix.API.Repositories.Services
{
    public class EmailClientService : IEmailClient
    {
        public Task SendEmailAsync(MailRequestService mailRequest)
        {
            throw new NotImplementedException();
        }
    }
}
