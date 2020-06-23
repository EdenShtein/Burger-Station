using Microsoft.EntityFrameworkCore.Migrations;

namespace Burger_Station.Migrations
{
    public partial class branches_orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ItemOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PostBody",
                table: "Comment",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_BranchId",
                table: "ItemOrder",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Branch_BranchId",
                table: "ItemOrder",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Branch_BranchId",
                table: "ItemOrder");

            migrationBuilder.DropIndex(
                name: "IX_ItemOrder_BranchId",
                table: "ItemOrder");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ItemOrder");

            migrationBuilder.AlterColumn<string>(
                name: "PostBody",
                table: "Comment",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);
        }
    }
}
