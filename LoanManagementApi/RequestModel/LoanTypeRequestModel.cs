namespace LoanManagementApi.RequestModel
{
    public class LoanTypeRequestModel
    {
        public string Name { get; set; }
        public decimal MaxAmount { get; set; }
        public int MaxDurationInMonths { get; set; }
    }
}
