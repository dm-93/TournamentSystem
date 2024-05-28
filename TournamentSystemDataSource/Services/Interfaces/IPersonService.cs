using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.DTO.Person.Request;
using TournamentSystemDataSource.DTO.Person.Response;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IPersonService
    {
        Task<CreatePersonResponse> CreateAsync(CreatePersonRequest createPersonRequest, CancellationToken cancellationToken);
        Task DeleteAsync(int personId, CancellationToken cancellationToken);
        Task<IEnumerable<GetPersonResponse>> GetByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<GetPersonResponse>> GetPersonsPaginatedAsync(Pagination pagination, CancellationToken cancellationToken);
        Task<UpdatePersonResponse> UpdateAsync(UpdatePersonRequest updatePersonRequest, CancellationToken cancellationToken);
    }
}