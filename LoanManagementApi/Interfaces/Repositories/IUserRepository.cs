using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<MyUser> GetByEmailAsync(string email);
        Task<MyUser> CreateAsync(MyUser user);
        Task<MyUser> GetByIdAsync(string id);
        Task<MyUser> GetByUsernameAsync(string username);
        Task<MyUser> UpdateAsync(MyUser user);
        Task<bool> DeleteAsync(string id);
    }
}
