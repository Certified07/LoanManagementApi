namespace LoanManagementApi.DTOs
{
    public class ClientResponseDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CreditScore { get; set; }
    }
}
