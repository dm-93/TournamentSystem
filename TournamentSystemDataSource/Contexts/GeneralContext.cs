using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Contexts
{
    public class GeneralContext : DbContext
    {
        public GeneralContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamDescription> TeamsDescriptions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Matchup> Matchups { get; set; }
        public DbSet<MatchupEntry> MatchupEntries { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<Pictures> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matchup>()
            .HasMany(m => m.Entries)
            .WithOne(e => e.ParentMatchup)
            .IsRequired();

            modelBuilder.Entity<MatchupEntry>()
                .HasOne(e => e.TeamCompeting)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matchup>()
                .HasOne(m => m.Winner)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.Deleted));
                    var condition = Expression.Lambda(Expression.Not(property), parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(condition);
                }
            }
        }
    }
}
