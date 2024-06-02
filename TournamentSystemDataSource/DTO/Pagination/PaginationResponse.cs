namespace TournamentSystemDataSource.DTO.Pagination
{
    public record PaginationResponse<TEntity> where TEntity : class
    {
        public int TotalCount { get; set; }
        public TEntity Data { get; set; }
    }
}
