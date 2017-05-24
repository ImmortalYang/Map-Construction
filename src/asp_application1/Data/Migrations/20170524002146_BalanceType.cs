using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace asp_application1.Data.Migrations
{
    public partial class BalanceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "Road",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "Pass",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "City",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "AspNetUsers",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Road",
                nullable: false);

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Pass",
                nullable: false);

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "City",
                nullable: false);

            migrationBuilder.AlterColumn<double>(
                name: "Balance",
                table: "AspNetUsers",
                nullable: false);
        }
    }
}
