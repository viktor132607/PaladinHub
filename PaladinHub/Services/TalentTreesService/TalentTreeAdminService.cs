using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Models;

namespace PaladinHub.Services.TalentTrees
{
	public class TalentTreeAdminService : ITalentTreeAdminService
	{
		private readonly AppDbContext _db;
		public TalentTreeAdminService(AppDbContext db) => _db = db;

		public async Task<IDictionary<string, bool>> GetStatesAsync(string treeKey)
		{
			return await _db.TalentNodeStates
				.Where(x => x.TreeKey == treeKey)
				.ToDictionaryAsync(x => x.NodeId, x => x.IsActive);
		}

		public async Task SetStatesAsync(string treeKey, IDictionary<string, bool> states)
		{
			var existing = await _db.TalentNodeStates.Where(x => x.TreeKey == treeKey).ToListAsync();

			// update + insert
			foreach (var kv in states)
			{
				var row = existing.FirstOrDefault(x => x.NodeId == kv.Key);
				if (row == null)
				{
					_db.TalentNodeStates.Add(new TalentNodeState
					{
						TreeKey = treeKey,
						NodeId = kv.Key,
						IsActive = kv.Value
					});
				}
				else
				{
					row.IsActive = kv.Value;
				}
			}

			// по желание: чистим записи за нодове, които вече не съществуват в states
			foreach (var row in existing)
			{
				if (!states.ContainsKey(row.NodeId))
					_db.TalentNodeStates.Remove(row);
			}

			await _db.SaveChangesAsync();
		}

		public async Task SetStateAsync(string treeKey, string nodeId, bool isActive)
		{
			var row = await _db.TalentNodeStates
				.FirstOrDefaultAsync(x => x.TreeKey == treeKey && x.NodeId == nodeId);

			if (row == null)
				_db.TalentNodeStates.Add(new TalentNodeState { TreeKey = treeKey, NodeId = nodeId, IsActive = isActive });
			else
				row.IsActive = isActive;

			await _db.SaveChangesAsync();
		}
	}
}
