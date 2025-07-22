using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface ILoanTypeService
    {
        Task<GetLoanTypeResponseModel> CreateAsync(LoanTypeRequestModel request);
        Task<BaseResponse> DeleteAsync(int id);
        Task<GetAllLoanTypesResponseModel> GetAllAsync();
        Task<GetLoanTypeResponseModel?> GetByIdAsync(int id);
        Task<GetLoanTypeResponseModel?> UpdateAsync(int id, LoanTypeRequestModel request);
    }
}