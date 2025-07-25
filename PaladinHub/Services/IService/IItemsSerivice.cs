using System.Collections.Generic;
using PaladinHub.Data.Entities;

namespace PaladinHub.Services.IService
{
	public interface IItemsService
	{
		List<Item> GetAllItems();
	}
}
