using Microsoft.AspNetCore.Mvc;
using PaladinHub.Services.IService;
using PaladinHub.Services.SectionServices;

namespace PaladinHub.Controllers
{
	[Route("{section:regex(^Holy$|^Protection$|^Retribution$)}")]
	public class PaladinController : BaseController
	{
		private readonly BaseSectionService _sectionService;

		public PaladinController(
			ISpellbookService spellbookService,
			IItemsService itemsService,
			HolySectionService holyService,
			ProtectionSectionService protectionService,
			RetributionSectionService retributionService,
			IHttpContextAccessor httpContextAccessor)
			: base(
				spellbookService,
				itemsService,
				PickSectionService(httpContextAccessor, holyService, protectionService, retributionService)
			)
		{
			_sectionService = (BaseSectionService)
				PickSectionService(httpContextAccessor, holyService, protectionService, retributionService);
		}

		private static ISectionService PickSectionService(
			IHttpContextAccessor accessor,
			HolySectionService holy,
			ProtectionSectionService prot,
			RetributionSectionService retri)
		{
			var section = accessor.HttpContext?
								  .GetRouteData()?
								  .Values["section"]?
								  .ToString();

			return section switch
			{
				"Holy" => holy,
				"Protection" => prot,
				"Retribution" => retri,
				_ => throw new ArgumentException($"Unknown section: '{section ?? "(null)"}'")
			};
		}

		[HttpGet("Overview")]
		public IActionResult Overview() => ViewWithCombinedData();

		[HttpGet("Talents")]
		public IActionResult Talents() => ViewWithCombinedData();

		[HttpGet("Stats")]
		public IActionResult Stats() => ViewWithCombinedData();

		[HttpGet("Consumables")]
		public IActionResult Consumables() => ViewWithCombinedData();

		[HttpGet("Gear")]
		public IActionResult Gear() => ViewWithCombinedData();

		[HttpGet("Rotation")]
		public IActionResult Rotation() => ViewWithCombinedData();
	}
}
