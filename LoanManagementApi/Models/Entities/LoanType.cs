using LoanManagementApi.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementApi.Models.Entities
{
    public class LoanType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(22,2)")]

        public decimal MaxAmount { get; set; }

        public int MaxDurationInMonths { get; set; }
        public RepaymentType RepaymentType { get; set; }

        public ICollection<Loan> Loans { get; set; } = [];
    }
}
