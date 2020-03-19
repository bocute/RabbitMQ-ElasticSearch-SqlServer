using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceRabbitMQ.Data.SqlServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notasfiscais",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Numero = table.Column<long>(nullable: false),
                    Serie = table.Column<string>(type: "varchar(3)", nullable: false),
                    SerieEcf = table.Column<string>(type: "varchar(20)", nullable: false),
                    NroEmpresa = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notasfiscais", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notasfiscais");
        }
    }
}
