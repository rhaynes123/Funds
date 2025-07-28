using System.ComponentModel.DataAnnotations;

namespace Funds.Domain.Entities;

public class FundRaiser
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    [StringLength(250)]
    public required string Name { get; init; }
    public decimal GoalAmount { get; init; }
    public required DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public virtual ICollection<Donation> Donations { get; init; } = [];
}