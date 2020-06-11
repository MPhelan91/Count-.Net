using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccessLayer.Migrations
{
    public partial class _103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodEntries");

            migrationBuilder.DropTable(
                name: "MealEntries");

            migrationBuilder.CreateTable(
                name: "CalorieEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Calories = table.Column<double>(nullable: false),
                    Protien = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalorieEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalorieEntries");

            migrationBuilder.CreateTable(
                name: "FoodEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calories = table.Column<double>(type: "float", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FoodForEntryId = table.Column<int>(type: "int", nullable: true),
                    Protien = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodEntries_SavedFoods_FoodForEntryId",
                        column: x => x.FoodForEntryId,
                        principalTable: "SavedFoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MealEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealForEntryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealEntries_SavedMeals_MealForEntryId",
                        column: x => x.MealForEntryId,
                        principalTable: "SavedMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntries_FoodForEntryId",
                table: "FoodEntries",
                column: "FoodForEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_MealEntries_MealForEntryId",
                table: "MealEntries",
                column: "MealForEntryId");
        }
    }
}
