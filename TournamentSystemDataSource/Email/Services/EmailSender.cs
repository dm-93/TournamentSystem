using MailKit.Net.Smtp;
using MimeKit;
using TournamentSystemDataSource.Email.Interfaces;
using TournamentSystemDataSource.Email.Models;

namespace TournamentSystemDataSource.Email.Services
{
    internal sealed class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message, CancellationToken cancellationToken)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage, cancellationToken);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true, cancellationToken);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
        }
    }
}
