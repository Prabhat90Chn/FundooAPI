using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, string recipientName, string link);
    }
}
