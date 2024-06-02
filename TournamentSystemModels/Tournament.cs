using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentSystemModels
{
    public class Tournament: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EntryFee { get; set; }
        public bool Completed { get; set; }
        public Pictures TournamentPicture { get; set; }
        public List<Team> EnteredTeams { get; set; } = [];
        public List<Prize> Prizes { get; set; } = [];
        public List<Matchup> Rounds { get; set; } = [];
        [NotMapped]
        public List<List<Matchup>> RoundsNm { get; set; } = [];
    }
}
