using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaladinHub.Services.TalentTrees
{
	public interface ITalentTreeAdminService
	{
		/// <summary>
		/// Връща речник {NodeId -> IsActive} за дадено дърво по ключ.
		/// </summary>
		Task<IDictionary<string, bool>> GetStatesAsync(string treeKey);

		/// <summary>
		/// Задава състояния за множество нодове наведнъж (замества или обновява).
		/// </summary>
		Task SetStatesAsync(string treeKey, IDictionary<string, bool> states);

		/// <summary>
		/// Задава състояние за конкретен нод.
		/// </summary>
		Task SetStateAsync(string treeKey, string nodeId, bool isActive);
	}
}
