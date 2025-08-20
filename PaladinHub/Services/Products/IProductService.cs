using PaladinHub.Data.Entities;
using PaladinHub.Models;
using PaladinHub.Models.Carts;
using PaladinHub.Models.Products;

namespace PaladinHub.Services.Products
{
	public interface IProductService
	{
		Task<ICollection<ProductViewModel>> GetAll();

		Task<MyCartViewModel> GetMyProducts(User user);

		Task<CreateProductViewModel> Create(CreateProductViewModel model);

		Task<bool> Delete(string id);

		Task<PagedResult<ProductViewModel>> QueryAsync(ProductQueryOptions options, CancellationToken ct = default);

		Task<List<string>> GetAllCategoriesAsync(CancellationToken ct = default);

		Task<EditProductViewModel?> GetForEditAsync(string id, CancellationToken ct = default);

		Task<bool> UpdateAsync(EditProductViewModel model, CancellationToken ct = default);
	}
}