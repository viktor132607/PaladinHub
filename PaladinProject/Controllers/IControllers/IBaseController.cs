using PaladinProject.Services.IService;

namespace PaladinProject.Controllers.IControllers
{
	public interface IBaseController
	{
		ISpellbookService SpellbookService { get; }
		IItemsService ItemsService { get; }

		string GetCoverImage();
	}
}
