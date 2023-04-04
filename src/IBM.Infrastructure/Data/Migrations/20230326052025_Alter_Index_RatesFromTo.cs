using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IBM.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Index_RatesFromTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_RATES_FROM_TO",
                table: "Rates",
                newName: "IX_Rates_From_To");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Rates_From_To",
                table: "Rates",
                newName: "IX_RATES_FROM_TO");
        }
    }
}
