using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, string recipientName, string link);
    }
}
