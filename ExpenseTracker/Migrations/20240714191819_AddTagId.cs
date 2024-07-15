using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddTagId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseTags_TagId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Expenses",
                newName: "Tagid");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_TagId",
                table: "Expenses",
                newName: "IX_Expenses_Tagid");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseTags_Tagid",
                table: "Expenses",
                column: "Tagid",
                principalTable: "ExpenseTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseTags_Tagid",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "Tagid",
                table: "Expenses",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_Tagid",
                table: "Expenses",
                newName: "IX_Expenses_TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseTags_TagId",
                table: "Expenses",
                column: "TagId",
                principalTable: "ExpenseTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
