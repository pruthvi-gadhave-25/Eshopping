using ProductService.DTO;
using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(CreateProduct createProduct);
        Task<Product> UpdateProductAsync(UpdateProduct updateProduct);
        Task<bool> DeleteProductAsync(int id);
    }
}
