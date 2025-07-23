using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface IClientService
    {
        Task<ClientResponseModel> GetAllClients();
        Task<GetClientResponseModel> GetClientWithLoansAsync(string clientId);
    }
}