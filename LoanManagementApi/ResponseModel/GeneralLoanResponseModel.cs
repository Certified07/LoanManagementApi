using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class GeneralLoanResponseModel : BaseResponse
    {
        public List<GeneralLoanDTO> Data { get; set; } = [];
    }
}
