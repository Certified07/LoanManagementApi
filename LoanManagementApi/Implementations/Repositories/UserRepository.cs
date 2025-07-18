using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyContext _context;

        public UserRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<MyUser> CreateAsync(MyUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<MyUser> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<MyUser> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<MyUser> UpdateAsync(MyUser user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.UserName = user.UserName;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Role = user.Role;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

