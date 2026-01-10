using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CodePulse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
       
        private readonly IWebHostEnvironment _env;
        private readonly IProductRepository productRepository ;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment env)
        {
            this.productRepository = productRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await productRepository.GetAllAsync();

            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ShortDescription = p.ShortDescription,
                Description = p.Description,
                Sku = p.Sku,
                Barcode = p.Barcode,
                Brand = p.Brand,
                Price = p.Price,
                OldPrice = p.OldPrice,
                CostPrice = p.CostPrice,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                IsFeatured = p.IsFeatured,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                Tags = p.Tags,
                ThumbnailImageUrl = p.ThumbnailImageUrl,
                ImageUrl1 = p.ImageUrl1,
                ImageUrl2 = p.ImageUrl2,
                ImageUrl4 = p.ImageUrl4,
                Color = p.Color,
                Size = p.Size,
                WeightKg = p.WeightKg,
                WidthCm = p.WidthCm,
                HeightCm = p.HeightCm,
                DepthCm = p.DepthCm
            });

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        public async Task<ActionResult<Product>> Create([FromForm] CreateProductRequestdto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required.");

            if (request.Price <= 0)
                return BadRequest("Price must be greater than 0.");

            // map DTO -> domain
            var product = new Product
            {
                Name = request.Name,
                ShortDescription = request.ShortDescription,
                Description = request.Description,
                Sku = request.Sku,
                Barcode = request.Barcode,
                Brand = request.Brand,
                Price = request.Price,
                OldPrice = request.OldPrice,
                CostPrice = request.CostPrice,
                StockQuantity = request.StockQuantity,
                IsActive = true,
                IsFeatured = false,
                Tags = request.Tags,
                Color = request.Color,
                Size = request.Size,
                WeightKg = request.WeightKg,
                WidthCm = request.WidthCm,
                HeightCm = request.HeightCm,
                DepthCm = request.DepthCm,
                CreatedAt = DateTime.UtcNow
            };

            if (request.CategoryId.HasValue)
            {
                product.CategoryId = request.CategoryId.Value;
            }

            // handle image (controller level)
            if (request.Image is not null && request.Image.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "products");
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                var ext = Path.GetExtension(request.Image.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                await using var stream = System.IO.File.Create(filePath);
                await request.Image.CopyToAsync(stream);

                var relativeUrl = $"/images/products/{fileName}";
                product.ThumbnailImageUrl = relativeUrl;
                product.ImageUrl1 = relativeUrl;
            }

            var created = await productRepository.CreateAsync(product, request.CategoryIds);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [RequestSizeLimit(10_000_000)]
        public async Task<ActionResult<Product>> Update(int id, [FromForm] CreateProductRequestdto request)
        {
            var existing = await productRepository.GetByIdAsync(id);
            if (existing is null) return NotFound();

            // map fields onto existing instance
            existing.Name = request.Name;
            existing.ShortDescription = request.ShortDescription;
            existing.Description = request.Description;
            existing.Sku = request.Sku;
            existing.Barcode = request.Barcode;
            existing.Brand = request.Brand;
            existing.Price = request.Price;
            existing.OldPrice = request.OldPrice;
            existing.CostPrice = request.CostPrice;
            existing.StockQuantity = request.StockQuantity;
            existing.Tags = request.Tags;
            existing.Color = request.Color;
            existing.Size = request.Size;
            existing.WeightKg = request.WeightKg;
            existing.WidthCm = request.WidthCm;
            existing.HeightCm = request.HeightCm;
            existing.DepthCm = request.DepthCm;
            existing.UpdatedAt = DateTime.UtcNow;

            if (request.CategoryId.HasValue)
            {
                existing.CategoryId = request.CategoryId.Value;
            }

            // new image (optional)
            if (request.Image is not null && request.Image.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "products");
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                var ext = Path.GetExtension(request.Image.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                await using var stream = System.IO.File.Create(filePath);
                await request.Image.CopyToAsync(stream);

                var relativeUrl = $"/images/products/{fileName}";
                existing.ThumbnailImageUrl = relativeUrl;
                existing.ImageUrl1 = relativeUrl;
            }

            var updated = await productRepository.UpdateAsync(existing, request.CategoryIds);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await productRepository.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
