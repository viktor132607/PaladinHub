using System;                             
using Microsoft.EntityFrameworkCore;    
using PaladinHub.Data;

namespace PaladinHub.Tests.Testing;

public static class DbContextFactory
{
	//public static AppDbContext CreateInMemory()
	//{
	//	var options = new DbContextOptionsBuilder<AppDbContext>()
	//		.UseInMemoryDatabase(Guid.NewGuid().ToString()) 
	//		.EnableSensitiveDataLogging()
	//		.Options;

	//	return new AppDbContext(options);
	//}
}
