using System.Threading.Tasks;
using PaladinHub.Data.Models;

namespace PaladinHub.Services.Pages
{
	public interface IPageService
	{
		Task<ContentPage?> GetByRouteAsync(string section, string slug);
		Task<bool> UpdateLayoutAsync(int id, string jsonLayout, string updatedBy);
	}
}
