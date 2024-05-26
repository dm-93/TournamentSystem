namespace TournamentSystemModels
{
    public class MatchupEntry
    {
        public int Id { get; set; }
        /// <summary>
        /// Represent one team in the matchup.
        /// </summary>
        public Team TeamCompeting { get; set; }
        /// <summary>
        /// Represents the score for this particular team.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Represents the matchup that this team came from as a winner.
        /// </summary>
        public Matchup ParentMatchup { get; set; }
    }
}