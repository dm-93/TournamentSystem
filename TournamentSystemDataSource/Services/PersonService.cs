using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.DTO;
using TournamentSystemDataSource.DTO.Pagination;
using TournamentSystemDataSource.DTO.Person.Request;
using TournamentSystemDataSource.DTO.Person.Response;
using TournamentSystemDataSource.Extensions;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class PersonService : IPersonService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<PersonService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public PersonService(GeneralContext context, ILogger<PersonService> logger, IUnitOfWork unitOfWork)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetPersonResponse>> GetPersonsPaginatedAsync(Pagination<int> pagination, CancellationToken cancellationToken)
        {
            var persons = await _context.Persons
                    .Include(p => p.Address)
                    .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                    .Take(pagination.ItemsPerPage)
                    .ToListAsync(cancellationToken);
            return persons.Select(p => new GetPersonResponse
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Weight = p.Weight,
                Gender = p.Gender,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
            });
        }

        public async Task<IEnumerable<GetPersonResponse>> GetByConditionAsync(GetByConditionRequest request, CancellationToken cancellationToken)
        {
            var property = typeof(Person).GetProperty(request.PropertyName,
                                                          BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase |
                                                          BindingFlags.FlattenHierarchy)
                ?? throw new ArgumentNullException($"Не получилось найти св-во: {request.PropertyName} в {nameof(Tournament)}.");
            return await _context.Persons
                .Include(p => p.Address)
                .ApplyFilter(property, request.PropertyValue, request.PropertyValueType)
                .Select(p => new GetPersonResponse
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age,
                    Weight = p.Weight,
                    Gender = p.Gender,
                    Phone = p.Phone,
                    Email = p.Email,
                    Address = p.Address,
                }).ToListAsync(cancellationToken);
        }

        public async Task<CreatePersonResponse> CreateAsync(CreatePersonRequest createPersonRequest, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(createPersonRequest);
            _logger.LogInformation("Creating a new person...");
            var person = new Person
            {
                FirstName = createPersonRequest.FirstName,
                LastName = createPersonRequest.LastName,
                Age = createPersonRequest.Age,
                Weight = createPersonRequest.Weight,
                Gender = createPersonRequest.Gender,
                Phone = createPersonRequest.Phone,
                Email = createPersonRequest.Email,
                TeamId = createPersonRequest.TeamId,
                Address = createPersonRequest.Address
            };
            var res = await _context.Persons.AddAsync(person);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"Person with id: {res.Entity.Id} created successfully.");
            return new CreatePersonResponse
            {
                Id = res.Entity.Id,
                FirstName = res.Entity.FirstName,
                LastName = res.Entity.LastName,
                Age = res.Entity.Age,
                Weight = res.Entity.Weight,
                Gender = res.Entity.Gender,
                Phone = res.Entity.Phone,
                Email = res.Entity.Email,
                TeamId = res.Entity.TeamId,
                Address = res.Entity.Address,
            };
        }

        public async Task<UpdatePersonResponse> UpdateAsync(UpdatePersonRequest updatePersonRequest, CancellationToken cancellationToken)
        {
            if (updatePersonRequest is null)
            {
                throw new ArgumentNullException($"{nameof(updatePersonRequest)} не может быть равен null.");
            }

            _logger.LogInformation($"Updating person with ID: {updatePersonRequest.Id}...");
            var person = await _context.Persons.FindAsync(updatePersonRequest.Id);

            if (person == null)
            {
                _logger.LogWarning($"Person with ID {updatePersonRequest.Id} not found.");
                throw new ArgumentException($"Персона с Id {updatePersonRequest.Id} не найдена.");
            }

            var properties = typeof(Person).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                           .Where(prop => prop.CanWrite);

            foreach (var property in properties)
            {
                var requestValue = typeof(UpdatePersonRequest).GetProperty(property.Name)?.GetValue(updatePersonRequest);
                if (requestValue != null)
                {
                    property.SetValue(person, requestValue);
                }
            }

            _context.Persons.Update(person);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"Person with ID {updatePersonRequest.Id} updated successfully.");
            return new UpdatePersonResponse
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
                Weight = person.Weight,
                Gender = person.Gender,
                Phone = person.Phone,
                Email = person.Email
            };
        }

        public async Task DeleteAsync(int personId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting person with ID: {personId}...");

            var person = await _context.Persons.FindAsync(personId);

            if (person == null)
            {
                _logger.LogWarning($"Person with ID {personId} not found.");
                throw new ArgumentException($"Персона с Id {personId} не найдена.");
            }

            _context.Persons.Remove(person);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Person with ID {personId} deleted successfully.");
        }
    }
}
