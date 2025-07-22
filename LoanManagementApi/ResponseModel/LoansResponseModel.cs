using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class LoansResponseModel : BaseResponse
    {
        public List<LoanDTO> Data { get; set; } = [];
    }
}
