using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        //HTTP POST METHOD TO CREATE A CATEGORY
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            await categoryRepository.CreateAsync(category);
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);


        }

        //https://localhost:44306/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });

            }
            return Ok(response);
        }

        //https://localhost:44306/api/Categories/{{id}}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            
            var category = await categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                var response = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                };
                
                return Ok(response);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditCategory(Guid id,  UpdateCategoryRequestdto updateCategoryRequestdto)
        {
            var category = new Category
            {
                Id = id,
                Name = updateCategoryRequestdto.Name,
                UrlHandle = updateCategoryRequestdto.UrlHandle
            };
            var updatedCategory = await categoryRepository.EdidAsync(category);

            if (updatedCategory == null)
            {
                return NotFound();
            }
            else
            {
                var response = new CategoryDto
                {
                    Id = updatedCategory.Id,
                    Name = updatedCategory.Name,
                    UrlHandle = updatedCategory.UrlHandle
                };
                return Ok(response);
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCategory( [FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }


    }
}
