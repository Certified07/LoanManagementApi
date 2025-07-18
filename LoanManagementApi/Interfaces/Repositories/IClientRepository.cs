using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task<Client> CreateAsync(Client client);
        Task<Client> GetByIdAsync(string id);
        Task<List<Client>> GetAllAsync();
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(string id);
        Task<Client> GetByEmailAsync(string email);
    }
}
