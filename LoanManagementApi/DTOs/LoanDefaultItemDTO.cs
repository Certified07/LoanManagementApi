namespace LoanManagementApi.DTOs
{
    public class LoanDefaultItemDTO
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public decimal Amount { get; set; }
        public decimal OutstandingBalance { get; set; }
        public DateTime DefaultDate { get; set; }
    }
}
