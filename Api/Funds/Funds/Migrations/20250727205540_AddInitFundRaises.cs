using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Funds.Migrations
{
    /// <inheritdoc />
    public partial class AddInitFundRaises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FundRaisers",
                columns: new[] { "Id", "Name", "GoalAmount", "StartDate", "EndDate" },
                values: new object[,]
                {
                    {
                        Guid.Parse("b3c0b8d5-0a6b-4e6f-94a2-d4a91fc49a01"),
                        "Community Park Renovation",
                        15000.00m,
                        new DateTime(2025, 7, 1),
                        new DateTime(2025, 9, 30)
                    },
                    {
                        Guid.Parse("9d28b730-4e8a-4a1e-95ae-66a19de5ab20"),
                        "School Supplies for Kids",
                        5000.00m,
                        new DateTime(2025, 8, 1),
                        new DateTime(2025, 8, 31)
                    },
                    {
                        Guid.Parse("d6f44dc6-1a4e-4dd5-a91f-5cfc87cbd024"),
                        "Animal Shelter Expansion",
                        25000.00m,
                        new DateTime(2025, 6, 15),
                        new DateTime(2025, 12, 31)
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FundRaisers",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    Guid.Parse("b3c0b8d5-0a6b-4e6f-94a2-d4a91fc49a01"),
                    Guid.Parse("9d28b730-4e8a-4a1e-95ae-66a19de5ab20"),
                    Guid.Parse("d6f44dc6-1a4e-4dd5-a91f-5cfc87cbd024")
                });
        }
    }
}
