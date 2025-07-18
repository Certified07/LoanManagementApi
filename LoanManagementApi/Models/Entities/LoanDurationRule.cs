using System.ComponentModel.DataAnnotations;

namespace LoanManagementApi.Models.Entities
{
    public class LoanDurationRule
    {
        public string Id { get; set; } 

        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }

        public int MaxDurationInMonths { get; set; }
    }
}
