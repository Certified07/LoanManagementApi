namespace LoanManagementApi.DTOs
{
    public class RepaymentHistoryResponseDTO
    {
        public Guid LoanId { get; set; }
        public List<RepaymentHistoryItemDTO> History { get; set; }
    }
}
