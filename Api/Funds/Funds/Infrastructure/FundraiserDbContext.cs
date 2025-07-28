using Funds.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Funds.Infrastructure;

public class FundraiserDbContext: DbContext
{
    public FundraiserDbContext(DbContextOptions<FundraiserDbContext> options) :base(options)
    {
        
    }
    public DbSet<FundRaiser> FundRaisers { get; set; } = null!;
    public DbSet<Donation> Donations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FundRaiser>()
            .HasMany(e => e.Donations)
            .WithOne(e => e.FundRaiser);
    }

}