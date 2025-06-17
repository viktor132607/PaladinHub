using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaladinProject.Models;

namespace PaladinProject.Data
{
	public class AppDbContext : IdentityDbContext<Users>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		
		

		{
			Database.EnsureCreated();
		}
	}

	//public DbSet<Spell> Spells { get; set; }

}

