using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product, IEnumerable<int> categoryIds)
        {
            // primary category from first selected if not set
            if (product.CategoryId == 0 && categoryIds.Any())
            {
                product.CategoryId = categoryIds.First();
            }

            // many-to-many categories
            if (categoryIds.Any())
            {
                var validIds = await dbContext.Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync();

                foreach (var catId in validIds)
                {
                    product.ProductCategories.Add(new ProductCategory
                    {
                        Product = product,
                        CategoryId = catId
                    });
                }
            }

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Product product, IEnumerable<int> categoryIds)
        {
            var existing = await dbContext.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existing == null)
                return null;

            // map scalar fields
            existing.Name = product.Name;
            existing.ShortDescription = product.ShortDescription;
            existing.Description = product.Description;
            existing.Sku = product.Sku;
            existing.Barcode = product.Barcode;
            existing.Brand = product.Brand;
            existing.Price = product.Price;
            existing.OldPrice = product.OldPrice;
            existing.CostPrice = product.CostPrice;
            existing.StockQuantity = product.StockQuantity;
            existing.IsActive = product.IsActive;
            existing.IsFeatured = product.IsFeatured;
            existing.Tags = product.Tags;
            existing.Color = product.Color;
            existing.Size = product.Size;
            existing.WeightKg = product.WeightKg;
            existing.WidthCm = product.WidthCm;
            existing.HeightCm = product.HeightCm;
            existing.DepthCm = product.DepthCm;
            existing.UpdatedAt = DateTime.UtcNow;

            if (product.CategoryId != 0)
            {
                existing.CategoryId = product.CategoryId;
            }

            // clear and re-add many-to-many categories
            existing.ProductCategories.Clear();

            if (categoryIds.Any())
            {
                var validIds = await dbContext.Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync();

                foreach (var catId in validIds)
                {
                    existing.ProductCategories.Add(new ProductCategory
                    {
                        ProductId = existing.Id,
                        CategoryId = catId
                    });
                }
            }

            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await dbContext.Products.FindAsync(id);
            if (existing == null)
                return false;

            dbContext.Products.Remove(existing);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
