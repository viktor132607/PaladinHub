using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PaladinHub.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

		var conn = Environment.GetEnvironmentVariable("DB_CONNECTION")
				   ?? "Host=localhost;Port=5432;Database=paladinhubdb;Username=postgres;Password=postgres;";

		optionsBuilder.UseNpgsql(conn);

		return new AppDbContext(optionsBuilder.Options);
	}
}
