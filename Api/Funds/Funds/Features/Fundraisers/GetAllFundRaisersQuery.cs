using Funds.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Funds.Features.Fundraisers;
public sealed record FundRaiserDto
{
    public required string Name { get; init; }
    public decimal GoalAmount { get; init; }
    public decimal CurrentAmount { get; init; }
    public required DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
public sealed record AllFundRaisersResponse(List<FundRaiserDto> FundRaisers);
public record GetAllFundRaisersQuery(): IRequest<IResult>;
public class GetAllFundRaisersQueryHandler(FundraiserDbContext dbContext): IRequestHandler<GetAllFundRaisersQuery, IResult>
{
    public async Task<IResult> Handle(GetAllFundRaisersQuery request, CancellationToken cancellationToken)
    {
        var fundRaisers = await dbContext.FundRaisers
            .AsNoTracking()
            .Select(fund => new FundRaiserDto
        {
            Name = fund.Name,
            GoalAmount = fund.GoalAmount,
            StartDate = fund.StartDate,
            CurrentAmount = fund.Donations.Select(d => d.Amount).Sum(),
            EndDate = fund.EndDate
        }).ToListAsync(cancellationToken);;
        var response = new AllFundRaisersResponse(fundRaisers);
        return Results.Ok(response);
    }
}

