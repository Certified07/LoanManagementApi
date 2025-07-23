namespace LoanManagementApi.DTOs
{
    public class ClientDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CreditScore {  get; set; }
        public int TotalLoans { get; set; }
        public int PaidLoans { get; set; }
        public decimal TotalLoanAmount { get; set; }
    }
}
