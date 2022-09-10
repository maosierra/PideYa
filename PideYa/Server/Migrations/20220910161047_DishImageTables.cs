using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PideYa.Server.Migrations
{
    public partial class DishImageTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DishImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dishid = table.Column<int>(type: "INTEGER", nullable: false),
                    url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_DishImages_Dishes_Dishid",
                        column: x => x.Dishid,
                        principalTable: "Dishes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishImages_Dishid",
                table: "DishImages",
                column: "Dishid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishImages");
        }
    }
}
