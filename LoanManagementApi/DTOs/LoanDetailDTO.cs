namespace LoanManagementApi.DTOs
{
    public class LoanDetailDTO
    {
        public string LoanId { get; set; }
        public decimal AmountRequested { get; set; }
        public decimal TotalAmountToRepay { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime? AprovalDate { get; set; }
        public string Status { get; set; }
        public int CreditScore {  get; set; }
    }
}
