using System.Diagnostics.CodeAnalysis;
using TournamentSystemDataSource.DTO.Admin;
using TournamentSystemModels;

namespace TournamentSystemDataSource.DTO.Person.Request
{
    public record CreatePersonRequest
    {
        public CreatePersonRequest()
        {
            
        }

        [SetsRequiredMembers]
        public CreatePersonRequest(AdminAddUserDto adminAddUserDto)
        {
            FirstName = adminAddUserDto.Name;
            LastName = adminAddUserDto.LastName;
            Age = adminAddUserDto.Age;
            Weight = adminAddUserDto.Weight;
            Gender = adminAddUserDto.Gender;
            Phone = adminAddUserDto.Phone;
            Email = adminAddUserDto.Email;
            TeamId = adminAddUserDto.TeamId;
            Address = adminAddUserDto.Address;
        }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int Age { get; set; }
        public double Weight { get; set; }
        public bool Gender { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required int TeamId { get; set; }
        public Address Address { get; set; }
    }
}
