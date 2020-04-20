using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCSDemoProjectAlcoaWebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryDetails",
                columns: table => new
                {
                    countryid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryname = table.Column<string>(nullable: true),
                    active_status = table.Column<bool>(nullable: false, defaultValue: true),
                    createddate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryDetails", x => x.countryid);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    roleid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rolename = table.Column<string>(nullable: true),
                    active_status = table.Column<bool>(nullable: false, defaultValue: true),
                    createddate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.roleid);
                });

            migrationBuilder.CreateTable(
                name: "StateDetails",
                columns: table => new
                {
                    stateid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    statename = table.Column<string>(nullable: false),
                    active_status = table.Column<bool>(nullable: false, defaultValue: true),
                    createddate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    countryidfk = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateDetails", x => x.stateid);
                    table.ForeignKey(
                        name: "FK_StateDetails_CountryDetails_countryidfk",
                        column: x => x.countryidfk,
                        principalTable: "CountryDetails",
                        principalColumn: "countryid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersDetailInfo",
                columns: table => new
                {
                    userid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    firstname = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    mobileno = table.Column<string>(nullable: true),
                    active_status = table.Column<bool>(nullable: false, defaultValue: false),
                    createdtime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    roleidfk = table.Column<long>(nullable: false),
                    cityidfk = table.Column<long>(nullable: false),
                    stateidfk = table.Column<long>(nullable: false),
                    countryidfk = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDetailInfo", x => x.userid);
                    table.ForeignKey(
                        name: "FK_UsersDetailInfo_UserRoles_roleidfk",
                        column: x => x.roleidfk,
                        principalTable: "UserRoles",
                        principalColumn: "roleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityDetails",
                columns: table => new
                {
                    cityid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cityname = table.Column<string>(nullable: true),
                    active_status = table.Column<bool>(nullable: false, defaultValue: true),
                    createddate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    stateidfk = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDetails", x => x.cityid);
                    table.ForeignKey(
                        name: "FK_CityDetails_StateDetails_stateidfk",
                        column: x => x.stateidfk,
                        principalTable: "StateDetails",
                        principalColumn: "stateid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityDetails_stateidfk",
                table: "CityDetails",
                column: "stateidfk");

            migrationBuilder.CreateIndex(
                name: "IX_StateDetails_countryidfk",
                table: "StateDetails",
                column: "countryidfk");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDetailInfo_roleidfk",
                table: "UsersDetailInfo",
                column: "roleidfk");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityDetails");

            migrationBuilder.DropTable(
                name: "UsersDetailInfo");

            migrationBuilder.DropTable(
                name: "StateDetails");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "CountryDetails");
        }
    }
}
