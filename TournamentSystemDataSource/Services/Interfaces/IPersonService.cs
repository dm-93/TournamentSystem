﻿using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Person.Request;
using TournamentSystemDataSource.DTO.Person.Response;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IPersonService
    {
        Task<CreatePersonResponse> CreateAsync(CreatePersonRequest createPersonRequest, CancellationToken cancellationToken);
        Task DeleteAsync(int personId, CancellationToken cancellationToken);
        Task<IEnumerable<GetPersonResponse>> GetByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken);
        Task<GetPersonResponse> GetByEmail(string email, CancellationToken cancellationToken);
        Task<IEnumerable<GetPersonResponse>> GetPersonsPaginatedAsync(Pagination<int> pagination, CancellationToken cancellationToken);
        Task<UpdatePersonResponse> UpdateAsync(UpdatePersonRequest updatePersonRequest, CancellationToken cancellationToken);
    }
}