using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task<Client> CreateAsync(Client client);
        Task<Client> GetByIdAsync(Guid id);
        Task<List<Client>> GetAllAsync();
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(Guid id);
        Task<Client> GetByEmailAsync(string email);
    }
}
