using PaladinHub.Services.IService;

namespace PaladinHub.Controllers.IControllers
{
	public interface IBaseController
	{
		ISpellbookService SpellbookService { get; }
		IItemsService ItemsService { get; }

		string GetCoverImage();
	}
}
