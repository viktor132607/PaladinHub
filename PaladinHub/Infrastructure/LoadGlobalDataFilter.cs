using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaladinHub.Services.IService;

namespace PaladinHub.Infrastructure
{
	public class LoadGlobalDataFilter : IAsyncActionFilter
	{
		private readonly ISpellbookService _spellbookService;
		private readonly IItemsService _itemsService;

		public LoadGlobalDataFilter(ISpellbookService spellbookService, IItemsService itemsService)
		{
			_spellbookService = spellbookService;
			_itemsService = itemsService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.Controller is Controller controller)
			{
				controller.ViewBag.AllSpells = await _spellbookService.GetAllAsync();
				controller.ViewBag.AllItems = await _itemsService.GetAllAsync();
			}

			await next();
		}
	}
}
