using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await categoryRepository.GetAllAsync();

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                ProductCount = c.Products?.Count ?? 0
            });

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                ProductCount = category.Products?.Count ?? 0
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            var created = await categoryRepository.CreateAsync(category);

            var result = new CategoryDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                IsActive = created.IsActive,
                ProductCount = 0
            };

            return CreatedAtAction(nameof(Get), new { id = created.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;

            var updated = await categoryRepository.UpdateAsync(category);

            var result = new CategoryDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                IsActive = updated.IsActive,
                ProductCount = updated.Products?.Count ?? 0
            };

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            await categoryRepository.DeleteAsync(category);
            return NoContent();
        }


    }
}
