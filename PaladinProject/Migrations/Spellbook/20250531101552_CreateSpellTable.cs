using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaladinProject.Migrations.Spellbook
{
    /// <inheritdoc />
    public partial class CreateSpellTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spell",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spell", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Spell",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Crusader Strike" },
                    { 2, "Judgement" },
                    { 3, "Divine Shield" },
                    { 4, "Hand Of Freedom" },
                    { 5, "Cleanse" },
                    { 6, "Blessing Of Protection" },
                    { 7, "Divine Steed" },
                    { 8, "Shield Of Righterous" },
                    { 9, "Lay Of Hands" },
                    { 10, "Flash Of Light" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spell");
        }
    }
}
