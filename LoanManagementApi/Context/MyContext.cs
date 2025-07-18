using LoanManagementApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MyContext : IdentityDbContext<MyUser>
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Repayment> Repayments { get; set; }
    public DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
    public DbSet<LoanDurationRule> LoanDurationRules { get; set; }
    public DbSet<MyUser> MyUsers { get; set; }
}
