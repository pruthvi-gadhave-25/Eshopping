using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTO;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _service.GetAllProductsAsync();
                return Ok(products);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _service.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the product.");
            }
        }
        


        
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProduct createProduct)
        {
            try
            {
                if (createProduct == null)
                    return BadRequest();

                var created = await _service.CreateProductAsync(createProduct);
                if (created == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create product.");

                return CreatedAtRoute("GetProductById", new { id = created.Id }, created);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProduct updateProduct)
        {
            try
            {
                if (updateProduct == null || updateProduct.Id != id)
                    return BadRequest();

                var updated = await _service.UpdateProductAsync(updateProduct);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteProductAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }
        }
    }
}
