using TournamentSystemModels;

namespace TournamentSystemDataSource.DTO.Person.Request
{
    public record CreatePersonRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int Age { get; set; }
        public required double Weight { get; set; }
        public required bool Gender { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public Address Address { get; set; }
    }
}
