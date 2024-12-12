using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeCampus.Migrations
{
    /// <inheritdoc />
    public partial class Refill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cleaning",
                columns: table => new
                {
                    CleaningID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Responsible = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cleaning", x => x.CleaningID);
                    table.ForeignKey(
                        name: "FK_Cleaning_Machine_MachineID",
                        column: x => x.MachineID,
                        principalTable: "Machine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cleaning_MachineID",
                table: "Cleaning",
                column: "MachineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cleaning");
        }
    }
}
