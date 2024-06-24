using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using TournamentSystemDataSource.Contexts;
using TournamentSystemDataSource.Email.Models;
using TournamentSystemDataSource.Extensions;
using TournamentSystemModels.Identity;

namespace TournamentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            }));

            #region Authentication
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthContext>()
                .AddApiEndpoints();
            builder.Services.AddAuthContext();
            #endregion

            builder.Services.AddGeneralContext();
            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection(nameof(EmailConfiguration)));
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<EmailConfiguration>>().Value);
            builder.Services.AddRepositories();
            builder.Services.AddUnitOfWork();
            builder.Services.AddServices();
            builder.Services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapIdentityApi<User>();
            app.MapControllers();
            app.Run();
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User", "Manager" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
