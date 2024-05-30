using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.Services;
using TournamentSystemDataSource.Services.Interfaces;

namespace TournamentSystemDataSource.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGeneralContext(this IServiceCollection services)
        {
            services.AddDbContext<GeneralContext>((sp, options) =>
            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Tournaments;Integrated Security=True;"));
        }

        public static void AddAuthContext(this IServiceCollection services)
        {
            services.AddDbContext<AuthContext>((sp, options) =>
            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TournamentsIdentity;Integrated Security=True;"));
        }

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPrizeService, PrizeService>();
            services.AddScoped<ITeamDescriptionService, TeamDescriptionService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPicturesService, PicturesService>();
            services.AddScoped<IRoundsService, RoundsService>();
        }
    }
}
