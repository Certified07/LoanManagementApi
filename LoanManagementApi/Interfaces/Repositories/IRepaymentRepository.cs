﻿using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IRepaymentRepository
    {
        Task<Repayment> CreateAsync(Repayment repayment);
        Task<Repayment> GetByIdAsync(Guid id);
        Task<List<Repayment>> GetHistoryByLoanIdAsync(Guid loanId);
        Task<Repayment> UpdateAsync(Repayment repayment);
        Task<bool> DeleteAsync(Guid id);
    }
}
