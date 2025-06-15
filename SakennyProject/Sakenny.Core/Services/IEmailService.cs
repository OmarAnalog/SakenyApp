using Sakenny.Core.DTO;

namespace Sakenny.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto emailDto);
    }
}
