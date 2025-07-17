using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Data
{
    // DbContext
    public class LoanManagementContext : DbContext
    {
        public LoanManagementContext(DbContextOptions<LoanManagementContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        public DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Client Configuration
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.CreditScore).IsRequired();
                entity.Property(e => e.Income).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });

            // Loan Configuration
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClientId).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.InterestRate).IsRequired();
                entity.Property(e => e.TermMonths).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.ApplicationDate).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.HasOne(e => e.Client)
                      .WithMany()
                      .HasForeignKey(e => e.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Repayment Configuration
            modelBuilder.Entity<Repayment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LoanId).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.PaymentDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Penalty).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(e => e.Loan)
                      .WithMany()
                      .HasForeignKey(e => e.LoanId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.RepaymentSchedules)
                      .WithOne(e => e.Repayment)
                      .HasForeignKey(e => e.RepaymentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // RepaymentSchedule Configuration
            modelBuilder.Entity<RepaymentSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LoanId).IsRequired();
                entity.Property(e => e.RepaymentId).IsRequired(false); // Nullable
                entity.Property(e => e.InstallmentNumber).IsRequired();
                entity.Property(e => e.DueDate).IsRequired();
                entity.Property(e => e.AmountDue).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(e => e.Loan)
                      .WithMany()
                      .HasForeignKey(e => e.LoanId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.LoanId, e.InstallmentNumber }).IsUnique();
            });

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                // Seed Admin User
                entity.HasData(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                        Role = UserRole.Admin,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            });
        }
    }
}