using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IUserRepository
    {

        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
