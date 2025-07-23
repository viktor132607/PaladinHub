using System.Collections.Generic;
using PaladinProject.Models;

namespace PaladinProject.Services.IService
{
	public interface IItemsService
	{
		List<Item> GetAllItems();
	}
}
