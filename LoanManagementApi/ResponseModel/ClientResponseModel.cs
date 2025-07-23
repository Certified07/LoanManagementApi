using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class ClientResponseModel : BaseResponse
    {
        public List<ClientDTO> Data { get; set; } = [];
    }
}
