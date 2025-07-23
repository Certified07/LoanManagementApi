using LoanManagementApi.DTOs;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILoanRepository _loanRepository;
        public ClientService(IClientRepository clientRepository, ILoanRepository loanRepository)
        {
            _clientRepository = clientRepository;
            _loanRepository = loanRepository;
        }
        public async Task<ClientResponseModel> GetAllClients()
        {
            var clients = await _clientRepository.GetAllAsync();


            var allLoans = await _loanRepository.GetAllAsync(); 

            var clientDtos = clients.Select(x => new ClientDTO
            {
                Id = x.Id,
                FullName = x.Name,
                Email = x.Email,
                PhoneNumber = x.Phone,
                CreditScore = x.CreditScore,
                TotalLoans = allLoans.Count(l => l.ClientId == x.Id),
                TotalLoanAmount = allLoans
                    .Where(l => l.ClientId == x.Id)
                    .Sum(l => l.TotalAmountToRepay)
            }).ToList();

            return new ClientResponseModel
            {
                Data = clientDtos,
                Message = "Sucessfully returned",
                Status = false
            };
        }
        public async Task<GetClientResponseModel> GetClientWithLoansAsync(string clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null) return null;

            var loans = await _loanRepository.GetAllAsync();
            var clientLoan = loans.Where(x => x.ClientId == clientId).ToList();
            var clientLoans = new List<LoanDetailDTO>();
            if (clientLoan != null)
            {
                clientLoans = clientLoan
                .Select(l => new LoanDetailDTO
                {
                    LoanId = l.Id,
                    AmountRequested = l.PrincipalAmount,
                    TotalAmountToRepay = l.TotalAmountToRepay,
                    InterestRate = l.InterestRate,
                    AprovalDate = l.ApprovalDate,
                    CreditScore = l.Client.CreditScore,
                    Status = l.Status.ToString()
                }).ToList();
            }
            

            return new GetClientResponseModel
            {
                Id = client.Id,
                FullName = client.Name,
                Email = client.Email,
                PhoneNumber = client.Phone,
                Loans = clientLoans
            };
        }

    }
}
