using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PaladinHub.Services.EmailSernderService
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtpServer = _configuration["Email:SmtpServer"];
			var port = int.Parse(_configuration["Email:Port"]);
			var senderEmail = _configuration["Email:Sender"];
			var password = _configuration["Email:Password"];

			var client = new SmtpClient(smtpServer)
			{
				Port = port,
				Credentials = new NetworkCredential(senderEmail, password),
				EnableSsl = true
			};

			return client.SendMailAsync(
				new MailMessage(senderEmail, email, subject, htmlMessage)
				{
					IsBodyHtml = true
				});
		}
	}
}
