using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Models;

namespace PaladinHub.Infrastructure
{
	/// <summary>
	/// Глобален филтър – зарежда списък с динамичните ContentPages,
	/// за да може _Layout да построи менюто (по секции).
	/// </summary>
	public class LoadGlobalDataFilter : IAsyncActionFilter
	{
		private readonly AppDbContext _db;
		public LoadGlobalDataFilter(AppDbContext db) => _db = db;

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Вземаме всички публикувани страници и ги групираме по секция.
			var pages = await _db.ContentPages
				.AsNoTracking()
				.Where(p => p.IsPublished)
				.OrderBy(p => p.Title)
				.ToListAsync();

			// Слагаме целия списък; в Layout ще филтрираме статичните slug-ове
			// и ще вземем само нужната секция.
			var controller = context.Controller as Controller;
			if (controller != null)
			{
				controller.ViewData["PagesBySection"] =
					pages.GroupBy(p => p.Section.Trim().ToLower())
						 .ToDictionary(g => g.Key, g => g.ToList());
			}

			await next();
		}
	}
}
