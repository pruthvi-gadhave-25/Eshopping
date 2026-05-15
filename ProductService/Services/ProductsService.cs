using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DTO;
using ProductService.Models;

namespace ProductService.Services
{
    public class ProductsService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductsService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(CreateProduct createProduct)
        {
            try
            {
                var product = new Product
                {
                    Name = createProduct.Name,
                    Description = createProduct.Description,
                    Price = createProduct.Price,
                    Stock = createProduct.Stock,
                    Category = createProduct.Category
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch
            {
                return Enumerable.Empty<Product>();
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Product> UpdateProductAsync(UpdateProduct updateProduct)
        {
            try
            {
                var existing = await _context.Products.FindAsync(updateProduct.Id);
                if (existing == null)
                    return null;

                existing.Name = updateProduct.Name;
                existing.Description = updateProduct.Description;
                existing.Price = updateProduct.Price;
                existing.Stock = updateProduct.Stock;
                existing.Category = updateProduct.Category;

                _context.Products.Update(existing);
                await _context.SaveChangesAsync();

                return existing;
            }
            catch
            {
                return null;
            }
        }
    }
}
