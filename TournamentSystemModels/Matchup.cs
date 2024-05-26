namespace TournamentSystemModels
{
    public class Matchup: BaseEntity
    {
        public List<MatchupEntry> Entries { get; set; } = new();
        public Team Winner { get; set; }
        public int MatchupRound { get; set; }
    }
}