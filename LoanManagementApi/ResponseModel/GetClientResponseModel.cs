using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class GetClientResponseModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<LoanDetailDTO> Loans { get; set; } = new();
    }
}
