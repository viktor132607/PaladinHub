using System.Collections.Generic;
using PaladinProject.Models;

namespace PaladinProject.Services
{
	public interface IItemsService
	{
		List<Item> GetAllItems();
	}
}
