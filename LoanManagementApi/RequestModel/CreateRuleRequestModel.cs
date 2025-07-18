namespace LoanManagementApi.RequestModel
{
    public class CreateRuleRequestModel
    {
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public int MaxDurationInMonths { get; set; }
    }
}
