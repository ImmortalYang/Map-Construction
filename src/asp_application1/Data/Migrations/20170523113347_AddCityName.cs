using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace asp_application1.Data.Migrations
{
    public partial class AddCityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "City",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "City");
        }
    }
}
