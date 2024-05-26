namespace TournamentSystemDataSource.DTO.Person.Request
{
    public record UpdatePersonRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
    }
}
