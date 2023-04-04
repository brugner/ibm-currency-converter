using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IBM.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_Index_TransactionsSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Sku",
                table: "Transactions",
                column: "Sku");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Sku",
                table: "Transactions");
        }
    }
}
