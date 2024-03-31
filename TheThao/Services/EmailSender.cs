using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace TheThao.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com", //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("clan.darknightes@gmail.com", "rywkcpnpfjezlvkq")
            };

            return client.SendMailAsync("clan.darknightes@gmail.com", email, subject, htmlMessage);
        }
    }
}
