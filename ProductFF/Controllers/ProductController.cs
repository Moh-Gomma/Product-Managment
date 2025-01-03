using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductFF.Db;
using ProductFF.Dtos;
using ProductFF.Model;

namespace ProductFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProductController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto.ViewProduct>>> GetAll()
        {
            var products = await context.Products
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    Category = p.Category != null ? new
                    {
                        p.Category.Id,
                        p.Category.Name
                    } : null 
                })
        .ToListAsync();
            return Ok(products);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProduct([FromQuery]string name , [FromQuery]int? id)
        {
            var query = context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            if (id.HasValue)
            {
                query = query.Where(p => p.Id == id.Value);
            }
            var result = await query.ToListAsync();
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Product>> AddNew(ProductDto.AddProduct productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = productDto.name,
                Price = productDto.price,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId

            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) { return NotFound(); }
            return Ok(product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto.UpdateProduct updateProductDTO)
        {
            if (id != updateProductDTO.id)
            {
                return BadRequest("ID mismatch");
            }

            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updateProductDTO.name;
            product.Description = updateProductDTO.Description;
            product.Price = updateProductDTO.price;
            product.CategoryId = updateProductDTO.CategoryId;

            context.Entry(product).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); 
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var pp = await context.Products.FindAsync(id);
            if (pp == null) { return NotFound(); };
            context.Products.Remove(pp);
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}
