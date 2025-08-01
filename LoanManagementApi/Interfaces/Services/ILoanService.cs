﻿using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface ILoanService
    {
        Task<ApplyLoanResponseModel> ApplyAsync(LoanRequestModel model);
        Task<BaseResponse> ApproveAsync(string loanId);
        Task<BaseResponse> RejectAsync(string loanId);
        Task<LoansResponseModel> GetDefaultedLoans();
        Task<LoansResponseModel> GetPaidLoans();
        Task<LoansResponseModel> GetOutstandingLoans();
        Task<GeneralLoanResponseModel> GetLoanDetailsForCurrentUser();
    }
}