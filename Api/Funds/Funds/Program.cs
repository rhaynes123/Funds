using Funds.Domain.Entities;
using Funds.Features.Donations;
using Funds.Features.Fundraisers;
using Funds.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web.Resource;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateDonationCommand)));
builder.Services.AddDbContext<FundraiserDbContext>(opt => opt.UseSqlite("Data Source=Funds.db"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FundraiserDbContext>();
    await db.Database.MigrateAsync();
}
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/api/fundraisers", async (IMediator mediator) => await mediator.Send(new GetAllFundRaisersQuery()))
    .WithOpenApi();
app.MapPost("/api/donate",
    async (IMediator mediator, DonationRequest request) => await mediator.Send(new CreateDonationCommand(request)))
    .WithOpenApi();

app.Run();
