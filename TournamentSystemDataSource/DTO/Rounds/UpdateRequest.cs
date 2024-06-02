using TournamentSystemModels;

namespace TournamentSystemDataSource.DTO.Rounds
{
    public class UpdateRequest
    {
        public int Id { get; set; }
        public List<MatchupEntry> Entries { get; set; } = new();
        public int Winner { get; set; }
        public int MatchupRound { get; set; }
        public int TournamentId { get; set; }
    }
}
