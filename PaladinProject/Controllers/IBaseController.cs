using PaladinProject.Services;

namespace PaladinProject.Controllers
{
	public interface IBaseController
	{
		ISpellbookService SpellbookService { get; }
		IItemsService ItemsService { get; }
	}
}
