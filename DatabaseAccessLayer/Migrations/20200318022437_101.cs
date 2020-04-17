using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccessLayer.Migrations
{
    public partial class _101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealEntry_SavedMeals_MealForEntryId",
                table: "MealEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealEntry",
                table: "MealEntry");

            migrationBuilder.RenameTable(
                name: "MealEntry",
                newName: "MealEntries");

            migrationBuilder.RenameIndex(
                name: "IX_MealEntry_MealForEntryId",
                table: "MealEntries",
                newName: "IX_MealEntries_MealForEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealEntries",
                table: "MealEntries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FoodEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    FoodForEntryId = table.Column<int>(nullable: true),
                    Calories = table.Column<int>(nullable: false),
                    Protien = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntries_FoodForEntryId",
                table: "FoodEntries",
                column: "FoodForEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEntries_SavedMeals_MealForEntryId",
                table: "MealEntries",
                column: "MealForEntryId",
                principalTable: "SavedMeals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealEntries_SavedMeals_MealForEntryId",
                table: "MealEntries");

            migrationBuilder.DropTable(
                name: "FoodEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealEntries",
                table: "MealEntries");

            migrationBuilder.RenameTable(
                name: "MealEntries",
                newName: "MealEntry");

            migrationBuilder.RenameIndex(
                name: "IX_MealEntries_MealForEntryId",
                table: "MealEntry",
                newName: "IX_MealEntry_MealForEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealEntry",
                table: "MealEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEntry_SavedMeals_MealForEntryId",
                table: "MealEntry",
                column: "MealForEntryId",
                principalTable: "SavedMeals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
