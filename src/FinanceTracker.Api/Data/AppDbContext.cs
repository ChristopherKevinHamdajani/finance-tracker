using Microsoft.EntityFrameworkCore;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BalanceSnapshot> BalanceSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(u => u.PasswordHash)
                .IsRequired();
            entity.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        // RefreshToken configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.TokenHash)
                .IsRequired();
            entity.Property(r => r.ExpiresAt)
                .IsRequired();
            entity.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
            entity.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Account configuration
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(a => a.Type)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.Currency)
                .IsRequired()
                .HasMaxLength(3);
            entity.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
            entity.HasIndex(a => a.UserId);
        });

        // Transaction configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            entity.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(t => t.Description)
                .HasMaxLength(255);
            entity.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
            entity.HasIndex(t => t.AccountId);
            entity.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // BalanceSnapshot configuration
        modelBuilder.Entity<BalanceSnapshot>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Balance)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            entity.Property(b => b.RecordedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
            entity.HasIndex(b => b.AccountId);
            entity.HasOne(b => b.Account)
                .WithMany(a => a.BalanceSnapshots)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}