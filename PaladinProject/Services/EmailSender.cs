using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace PaladinProject.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string message)
		{
			var email = new MimeMessage();

			email.From.Add(MailboxAddress.Parse("paladinhubemail@gmail.com"));
			email.To.Add(MailboxAddress.Parse(toEmail));
			email.Subject = subject;
			email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync("paladinhubemail@gmail.com", _configuration["Email:Password"]);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}
