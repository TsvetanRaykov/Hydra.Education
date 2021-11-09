using Microsoft.EntityFrameworkCore.Migrations;

namespace Hydra.Server.Auth.Data.Migrations
{
    public partial class AddHydraIdentityNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "AspNetUsers");
        }
    }
}
