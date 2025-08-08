using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaladinHub.Migrations
{
	public partial class AddCartAndRelations : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// 1) Добави CartId към AspNetUsers (nullable, за да не чупи регистрацията)
			migrationBuilder.AddColumn<string>(
				name: "CartId",
				table: "AspNetUsers",
				type: "text",
				nullable: true);

			// 2) Products
			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<string>(type: "text", nullable: false),
					Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
				});

			// 3) Carts
			migrationBuilder.CreateTable(
				name: "Carts",
				columns: table => new
				{
					Id = table.Column<string>(type: "text", nullable: false),
					IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
					OrderDate = table.Column<string>(type: "text", nullable: true),
					UserId = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Carts", x => x.Id);
					table.ForeignKey(
						name: "FK_Carts_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			// 4) CartProduct (join + quantity)
			migrationBuilder.CreateTable(
				name: "CartProduct",
				columns: table => new
				{
					ProductId = table.Column<string>(type: "text", nullable: false),
					CartId = table.Column<string>(type: "text", nullable: false),
					Quantity = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CartProduct", x => new { x.ProductId, x.CartId });
					table.ForeignKey(
						name: "FK_CartProduct_Carts_CartId",
						column: x => x.CartId,
						principalTable: "Carts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CartProduct_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			// Индекси
			migrationBuilder.CreateIndex(
				name: "IX_Carts_UserId",
				table: "Carts",
				column: "UserId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_CartProduct_CartId",
				table: "CartProduct",
				column: "CartId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_CartId",
				table: "AspNetUsers",
				column: "CartId",
				unique: true);

			// 5) FK от AspNetUsers.CartId към Carts.Id
			migrationBuilder.AddForeignKey(
				name: "FK_AspNetUsers_Carts_CartId",
				table: "AspNetUsers",
				column: "CartId",
				principalTable: "Carts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUsers_Carts_CartId",
				table: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "CartProduct");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Carts");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_CartId",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "CartId",
				table: "AspNetUsers");
		}
	}
}
