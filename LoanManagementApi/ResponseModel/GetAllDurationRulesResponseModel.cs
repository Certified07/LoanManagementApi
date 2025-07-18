using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class GetAllDurationRulesResponseModel : BaseResponse
    {
        public List<LoanDurationDTO> Data { get; set; } = [];
    }
}
