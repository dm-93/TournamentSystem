namespace TournamentSystemModels
{
    public class Team: BaseEntity
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public TeamDescription Description { get; set; }
        public ICollection<Person> TeamMembers { get; set; } = new List<Person>();
    }
}
