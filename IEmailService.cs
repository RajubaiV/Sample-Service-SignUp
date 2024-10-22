using PageSignup.Models;

namespace PageSignup
{
    public interface IEmailService
    {
        Task SendEmail(Mailrequest mailrequest);
    }
}
