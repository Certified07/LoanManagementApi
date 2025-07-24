using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.DTOs
{
    public class GeneralLoanDTO
    {
        public string LoanId { get; set; }
        public string Username { get; set; }
        public string Status {  get; set; }
        public List<RepaymentScheduleDTO> Schedules { get; set; }
    }
}
