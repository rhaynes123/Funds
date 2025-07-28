using System.ComponentModel.DataAnnotations;

namespace Funds.Domain.Entities;

public class Donation
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    [StringLength(400)]
    public required string Name { get; init; }
    public required FundRaiser FundRaiser { get; init; }
    public decimal Amount { get; init; }
    public DateTime CreatedAt { get; init; }
}