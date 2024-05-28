namespace TournamentSystemModels
{
    public class Person: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public int TeamId { get; set; }
        public Pictures UserPicture { get; set; }
        public Address Address { get; set; }
    }
}
