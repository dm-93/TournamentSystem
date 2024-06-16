using TournamentSystemDataSource.Email.Models;

namespace TournamentSystemDataSource.Email.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message, CancellationToken cancellationToken);
    }
}
