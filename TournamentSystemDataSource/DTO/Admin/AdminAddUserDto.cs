using TournamentSystemModels;

namespace TournamentSystemDataSource.DTO.Admin
{
    public record AdminAddUserDto(string Name, string Password)
    {
        public string LastName { get; init; }
        public int Age { get; init; }
        public double Weight { get; init; }
        public bool Gender { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
        public Address Address { get; init; }
        public int TeamId { get; init; }
        public string UserPictureBase64 { get; set; }
    }
}
