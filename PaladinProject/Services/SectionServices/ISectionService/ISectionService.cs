using PaladinProject.ViewModels;
using System.Collections.Generic;

namespace PaladinProject.Services.IService
{
	public interface ISectionService
	{
		string GetCoverImage();

		string GetPageTitle(string actionName);
		string GetPageText(string actionName);

		List<NavButton> GetCurrentSectionButtons(string actionName);

		List<NavButton> GetOtherSectionButtons();
	}
}
