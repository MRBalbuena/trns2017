using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trns.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransKey = table.Column<string>(maxLength: 200, nullable: false),
                    Text = table.Column<string>(maxLength: 2000, nullable: false),
                    Spanish = table.Column<string>(nullable: true),
                    BlockedBy = table.Column<string>(maxLength: 50, nullable: true),
                    BlockedTime = table.Column<DateTime>(nullable: true),
                    TransBy = table.Column<string>(maxLength: 50, nullable: true),
                    TransDate = table.Column<DateTime>(nullable: true),
                    CheckedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CheckedTime = table.Column<DateTime>(nullable: true),
                    EditedBy = table.Column<string>(maxLength: 50, nullable: true),
                    EditedTime = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(maxLength: 500, nullable: true),
                    Edition = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translations");
        }
    }
}
