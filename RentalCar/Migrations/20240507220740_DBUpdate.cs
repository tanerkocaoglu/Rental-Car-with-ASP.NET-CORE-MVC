using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCar.Migrations
{
    /// <inheritdoc />
    public partial class DBUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Districts_DistrictID",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Branches_BranchID",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Branches_BranchID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BranchID",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Districts",
                table: "Districts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "Districts",
                newName: "District");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branch");

            migrationBuilder.RenameIndex(
                name: "IX_Branches_DistrictID",
                table: "Branch",
                newName: "IX_Branch_DistrictID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_District",
                table: "District",
                column: "DistrictID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branch",
                table: "Branch",
                column: "BranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_District_DistrictID",
                table: "Branch",
                column: "DistrictID",
                principalTable: "District",
                principalColumn: "DistrictID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Branch_BranchID",
                table: "Reservations",
                column: "BranchID",
                principalTable: "Branch",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_District_DistrictID",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Branch_BranchID",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_District",
                table: "District");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branch",
                table: "Branch");

            migrationBuilder.RenameTable(
                name: "District",
                newName: "Districts");

            migrationBuilder.RenameTable(
                name: "Branch",
                newName: "Branches");

            migrationBuilder.RenameIndex(
                name: "IX_Branch_DistrictID",
                table: "Branches",
                newName: "IX_Branches_DistrictID");

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Districts",
                table: "Districts",
                column: "DistrictID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BranchID",
                table: "Cars",
                column: "BranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Districts_DistrictID",
                table: "Branches",
                column: "DistrictID",
                principalTable: "Districts",
                principalColumn: "DistrictID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Branches_BranchID",
                table: "Cars",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Branches_BranchID",
                table: "Reservations",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
