using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class GetAllLoanTypesResponseModel : BaseResponse
    {
        public List<LoanTypeDTO> Data { get; set; } = new List<LoanTypeDTO>();
    }
}
