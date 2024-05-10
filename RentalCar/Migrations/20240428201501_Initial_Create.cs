using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCar.Migrations
{
	/// <inheritdoc />
	public partial class Initial_Create : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Districts",
				columns: table => new
				{
					DistrictID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					DistrictName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Districts", x => x.DistrictID);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.UserId);
				});

			migrationBuilder.CreateTable(
				name: "Branches",
				columns: table => new
				{
					BranchID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					DistrictID = table.Column<int>(type: "int", nullable: false),
					Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Branches", x => x.BranchID);
					table.ForeignKey(
						name: "FK_Branches_Districts_DistrictID",
						column: x => x.DistrictID,
						principalTable: "Districts",
						principalColumn: "DistrictID",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Cars",
				columns: table => new
				{
					CarID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BranchID = table.Column<int>(type: "int", nullable: false),
					Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Year = table.Column<int>(type: "int", nullable: false),
					FuelType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					TransmissionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					DailyRate = table.Column<int>(type: "int", nullable: false),
					Availability = table.Column<bool>(type: "bit", nullable: false),
					ImageFile = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Cars", x => x.CarID);
					table.ForeignKey(
						name: "FK_Cars_Branches_BranchID",
						column: x => x.BranchID,
						principalTable: "Branches",
						principalColumn: "BranchID",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Reservations",
				columns: table => new
				{
					ReservationID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserID = table.Column<int>(type: "int", nullable: false),
					CarID = table.Column<int>(type: "int", nullable: false),
					BranchID = table.Column<int>(type: "int", nullable: false),
					PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					TotalPrice = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Reservations", x => x.ReservationID);
					table.ForeignKey(
						name: "FK_Reservations_Branches_BranchID",
						column: x => x.BranchID,
						principalTable: "Branches",
						principalColumn: "BranchID",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Reservations_Cars_CarID",
						column: x => x.CarID,
						principalTable: "Cars",
						principalColumn: "CarID",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Reservations_Users_UserID",
						column: x => x.UserID,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Branches_DistrictID",
				table: "Branches",
				column: "DistrictID");

			migrationBuilder.CreateIndex(
				name: "IX_Cars_BranchID",
				table: "Cars",
				column: "BranchID");

			migrationBuilder.CreateIndex(
				name: "IX_Reservations_BranchID",
				table: "Reservations",
				column: "BranchID");

			migrationBuilder.CreateIndex(
				name: "IX_Reservations_CarID",
				table: "Reservations",
				column: "CarID");

			migrationBuilder.CreateIndex(
				name: "IX_Reservations_UserID",
				table: "Reservations",
				column: "UserID");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Reservations");

			migrationBuilder.DropTable(
				name: "Cars");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "Branches");

			migrationBuilder.DropTable(
				name: "Districts");
		}
	}
}
