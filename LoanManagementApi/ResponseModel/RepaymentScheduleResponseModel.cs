using LoanManagementApi.DTOs;

namespace LoanManagementApi.ResponseModel
{
    public class RepaymentScheduleResponseModel
    {
        public string LoanId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPenalty { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Outstanding { get; set; }
        public List<RepaymentScheduleDTO> Schedules { get; set; }
    }
}
