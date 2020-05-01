using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Data.Migrations
{
    public partial class Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, type: "INT IDENTITY(1,1)"),
                    CategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryName" },
                values: new object[] { "Автомобили" });
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryName" },
                values: new object[] { "Книги" });
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryName" },
                values: new object[] { "Мебель" });
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryName" },
                values: new object[] { "Услуги" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Categories");
        }
    }
}
