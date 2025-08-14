using System.Threading.Tasks;

namespace PaladinHub.Services.PageBuilder
{
	public interface IBlockRenderer
	{
		Task<string> RenderAsync(string jsonLayout);
	}
}
