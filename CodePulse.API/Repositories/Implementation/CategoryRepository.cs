using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync ()
        {
            return await dbContext.Categories.ToListAsync();
        
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category?> EdidAsync(Category category)
        {
            var updatedCategory = await dbContext.Categories.FindAsync(category.Id);
            if (updatedCategory == null)
            {
                return null;
            }
            else
            {
                dbContext.Entry(updatedCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
               

                return updatedCategory;
            }

        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (existingCategory is null)
            {
                return null; 
            }
            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();

            return existingCategory;

        }
    }
}
