using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;
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
	}
}