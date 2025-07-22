using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class GetLoanTypeResponseModel : BaseResponse
    {
        public LoanTypeDTO Data { get; set; }
    }
}
