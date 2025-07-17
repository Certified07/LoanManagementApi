using LoanManagementApi.Data;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Repositories.Implementations
{
    public class ClientRepository : IClientRepository
    {
        private readonly LoanManagementContext _context;

        public ClientRepository(LoanManagementContext context)
        {
            _context = context;
        }

        public async Task<Client> CreateAsync(Client client)
        {
            client.Id = Guid.NewGuid();
            client.CreatedAt = DateTime.UtcNow;
            client.UpdatedAt = DateTime.UtcNow;
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> UpdateAsync(Client client)
        {
            var existingClient = await _context.Clients.FindAsync(client.Id);
            if (existingClient == null)
            {
                return null;
            }

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.Email = client.Email;
            existingClient.Phone = client.Phone;
            existingClient.CreditScore = client.CreditScore;
            existingClient.Income = client.Income;
            existingClient.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingClient;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
