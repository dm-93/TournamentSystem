namespace TournamentSystemModels
{
    public class Prize: BaseEntity
    {
        public int PlaceNr { get; set; }
        public string PlaceName { get; set; }
        public decimal PrizePercentage { get; set; }
        public decimal PrizeAmount { get; set; }
    }
}