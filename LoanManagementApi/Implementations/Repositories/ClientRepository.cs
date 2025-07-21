using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly MyContext _context;

        public ClientRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> GetByIdAsync(string id)
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

            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.Phone = client.Phone;
            existingClient.CreditScore = client.CreditScore;
            existingClient.Income = client.Income;

            await _context.SaveChangesAsync();
            return existingClient;
        }

        public async Task<bool> DeleteAsync(string id)
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
