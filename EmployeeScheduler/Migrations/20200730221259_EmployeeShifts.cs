using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeScheduler.Migrations
{
    public partial class EmployeeShifts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeShifts",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    EmployeeUID = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShifts", x => x.UID);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Employees_EmployeeUID",
                        column: x => x.EmployeeUID,
                        principalTable: "Employees",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_EmployeeUID",
                table: "EmployeeShifts",
                column: "EmployeeUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeShifts");
        }
    }
}
