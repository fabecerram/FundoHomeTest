using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.WebApi.Models;

public partial class FundoLoanContext : IdentityDbContext<ApplicationUser>
{
    public FundoLoanContext(DbContextOptions<FundoLoanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Loan> Loans { get; set; }

    public DbSet<TokenInfo> TokenInfos { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("Loan");

            entity.Property(e => e.Amount).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.ApplicantName).HasMaxLength(50);
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(19, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}