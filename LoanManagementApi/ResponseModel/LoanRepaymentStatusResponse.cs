namespace LoanManagementApi.ResponseModel
{
    public class LoanRepaymentStatusResponse : BaseResponse
    {
        public int PrincipalAmount { get; set; }
        public int Interest { get; set; }
        public int TotalAmountToRepay { get; set; }
        public int Paid { get; set; }
        public int Penalty { get; set; }
        public int Outstanding { get; set; }
        public DateOnly DueDate { get; set; }
        public bool IsFullyPaid { get; set; }
    }
}
