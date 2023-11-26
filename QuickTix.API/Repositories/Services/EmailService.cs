using HouseMate.API.Entities.Services;
using HouseMate.API.Repositories.Interfaces;

namespace HouseMate.API.Repositories.Services
{
    public class EmailService : IEmailClient
    {
        public Task SendEmailAsync(MailRequestService mailRequest)
        {
            throw new NotImplementedException();
        }
    }
}
