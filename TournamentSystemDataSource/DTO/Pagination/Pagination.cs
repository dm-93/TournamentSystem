namespace TournamentSystemDataSource.DTO.Pagination
{
    public record Pagination<TEntity>
    {
        public required int Page { get; set; } = 1;
        public required int ItemsPerPage { get; set; } = 0;
        public TEntity? Parameter { get; set; }
    }
}
