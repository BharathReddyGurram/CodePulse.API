using CodePulse.API.Models.Domain;
using System.Collections.Generic;

namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetById(Guid id);
        Task<Category?> EdidAsync(Category category);
        Task<Category?>  DeleteAsync(Guid id);
    }
}
