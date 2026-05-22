namespace FinanceTracker.Api.Models;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Currency { get; set; } = "NZD";
    public string Status { get; set; } = "ACTIVE";
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<BalanceSnapshot> BalanceSnapshots { get; set; } = new List<BalanceSnapshot>();
}