namespace TournamentSystemDataSource.DTO
{
    public record Pagination
    {
        public required int Take { get; set; } = 10;
        public required int Skip { get; set; } = 0;
    }
}
