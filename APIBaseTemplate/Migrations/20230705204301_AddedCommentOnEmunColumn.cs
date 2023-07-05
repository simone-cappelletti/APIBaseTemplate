using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBaseTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommentOnEmunColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PriceType",
                table: "FligthServices",
                type: "nvarchar(max)",
                nullable: false,
                comment: "FlightServiceType values (Fligth = 0, HandLuggage = 1, HoldLuggage = 2, FligthInsurance = 3)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PriceType",
                table: "FligthServices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "FlightServiceType values (Fligth = 0, HandLuggage = 1, HoldLuggage = 2, FligthInsurance = 3)");
        }
    }
}
