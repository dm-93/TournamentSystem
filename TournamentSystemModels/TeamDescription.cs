namespace TournamentSystemModels
{
    public class TeamDescription: BaseEntity
    {
        public string Description { get; set; }
        public int? SchoolNr { get; set; }
        public Pictures TeamPicture { get; set; }
    }
}
