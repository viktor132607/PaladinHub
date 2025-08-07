

namespace PaladinHub.ServiceExtensions
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddCustomServices(this IServiceCollection services)
		{
			// DATA SEEDER
			services.AddScoped<DataSeeder>();

			// SERVICES
			services.AddTransient<I_Service, _Service>();
			services.AddTransient<IMatchService, MatchService>();

			// REPOS
			services.AddScoped<I_Repository, _Repository>();
			services.AddScoped<IMatchRepository, MatchRepository>();

			return services;
		}
	}
}