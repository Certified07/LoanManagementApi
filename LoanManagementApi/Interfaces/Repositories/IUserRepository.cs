using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<MyUser> CreateAsync(MyUser user);
        Task<MyUser> GetByIdAsync(Guid id);
        Task<MyUser> GetByUsernameAsync(string username);
        Task<MyUser> UpdateAsync(MyUser user);
        Task<bool> DeleteAsync(Guid id);
    }
}
