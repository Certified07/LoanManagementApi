using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Repositories.Interfaces
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
