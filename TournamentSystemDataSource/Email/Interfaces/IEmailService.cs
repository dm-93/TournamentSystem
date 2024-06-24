using TournamentSystemDataSource.DTO.Person.Response;

namespace TournamentSystemDataSource.Email.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(CreatePersonResponse personInfo, CancellationToken cancellationToken);
    }
}
