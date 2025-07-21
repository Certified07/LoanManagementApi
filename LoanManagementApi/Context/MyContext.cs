using LoanManagementApi.Implementations.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
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
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var adminRoleId = Guid.NewGuid().ToString();
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Client"
            }
        );
        var adminUser = new MyUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Admin",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            Role = UserRole.Admin,
            PasswordHash = UserService.HashPassword("admin")
        };

        builder.Entity<MyUser>().HasData(adminUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = adminUser.Id
        });
    }

}
