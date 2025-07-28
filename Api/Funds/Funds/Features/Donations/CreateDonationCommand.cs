using Funds.Domain.Entities;
using Funds.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Funds.Features.Donations;

public sealed record DonationRequest(string? FundRaiserName, string? Name, decimal? Amount, DateTime? CreatedAt);
public record CreateDonationCommand(DonationRequest DonationRequest ) : IRequest<IResult>;
public class CreateDonationCommandHandler(FundraiserDbContext dbContext) : IRequestHandler<CreateDonationCommand, IResult>
{
    public async Task<IResult> Handle(CreateDonationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.DonationRequest.FundRaiserName))
            {
                return Results.BadRequest("FundRaiserId is invalid");
            }
            var funRaiser = await dbContext.FundRaisers
                .FirstOrDefaultAsync( f => f.Name == request.DonationRequest.FundRaiserName, cancellationToken);
            if (funRaiser is null)
            {
                return Results.NotFound();
            }
            var entity = await dbContext.Donations.AddAsync(new Donation
            {
                FundRaiser = funRaiser,
                Name = request.DonationRequest.Name,
                Amount = (decimal)request.DonationRequest.Amount,
                CreatedAt = DateTime.UtcNow,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.Created("Donation", entity.Entity.Id);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError("Error creating donation");
        }
    }
}