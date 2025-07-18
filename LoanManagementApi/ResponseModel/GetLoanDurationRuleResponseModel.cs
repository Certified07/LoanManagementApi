using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class GetLoanDurationRuleResponseModel : BaseResponse
    {
        public LoanDurationDTO Data { get; set; }
    }
}
