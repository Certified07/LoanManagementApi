namespace LoanManagementApi.DTOs
{
    public class RepaymentScheduleResponseDTO
    {
        public Guid LoanId { get; set; }
        public List<RepaymentScheduleItemDTO> Schedule { get; set; }
    }
}
