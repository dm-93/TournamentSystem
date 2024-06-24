using TournamentSystemDataSource.DTO.Person.Response;
using TournamentSystemDataSource.Email.Interfaces;
using TournamentSystemDataSource.Email.Models;

namespace TournamentSystemDataSource.Email.Services
{
    internal class EmailService: IEmailService
    {
        private readonly IEmailSender _sender;

        public EmailService(IEmailSender sender)
        {
            _sender = sender;
        }

        public async Task SendEmailAsync(CreatePersonResponse personInfo, CancellationToken cancellationToken)
        {
            var messageContent = $"Здравствуйте {personInfo.FirstName}. Вы были добавлены в приложение как участник соревнования.";
            var message = new Message(new List<string> { personInfo.Email }, "Вы были добавлены как участник.", messageContent);
            await _sender.SendEmailAsync(message, cancellationToken);
        }
    }
}
