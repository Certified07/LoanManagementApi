namespace LoanManagementApi.DTOs
{
    public class LoanTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MaxAmount { get; set; }
        public int MaxDurationInMonths { get; set; }
    }
}
