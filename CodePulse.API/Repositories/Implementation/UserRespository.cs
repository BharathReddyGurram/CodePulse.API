using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodePulse.API.Repositories.Implementation
{
    public class UserRespository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRespository(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await dbContext.Users.AnyAsync(u => u.Email == email);
        }
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
