namespace FinanceTracker.Api.Models;

public class BalanceSnapshot
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public DateTime RecordedAt { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
}