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
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto.ViewCategory>>> GetAll()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto.ViewCategory(c.Id, c.Name, c.Description))
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Category>>> SearchCategories([FromQuery] string name, [FromQuery] int? id)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (id.HasValue)
            {
                query = query.Where(c => c.Id == id.Value);
            }

            var results = await query.ToListAsync();
            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> AddNew(CategoryDto.AddCategory categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new CategoryDto.ViewCategory(category.Id, category.Name, category.Description));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto.AddCategory categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Ok(new CategoryDto.ViewCategory(category.Id, category.Name, category.Description));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
