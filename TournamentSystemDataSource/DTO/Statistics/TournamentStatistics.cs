using TournamentSystemModels;

namespace TournamentSystemDataSource.DTO.Statistics
{
    public sealed class TournamentStatistics
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int CountOfRounds { get; set; }
        public int CountOfMatches { get; set; }
        public Team Winner { get; set; }
        public Dictionary<string, double> TeamAverageScoreStatistic { get; set; }
    }
}
