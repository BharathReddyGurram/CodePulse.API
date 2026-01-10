using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);

        Task<Product> CreateAsync(Product product, IEnumerable<int> categoryIds);
        Task<Product?> UpdateAsync(Product product, IEnumerable<int> categoryIds);
        Task<bool> DeleteAsync(int id);
    }
}
