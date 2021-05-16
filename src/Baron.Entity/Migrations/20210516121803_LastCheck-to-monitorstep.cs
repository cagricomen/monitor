﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Baron.Entity.Migrations
{
    public partial class LastChecktomonitorstep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCheckDate",
                table: "MonitorStep",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCheckDate",
                table: "MonitorStep");
        }
    }
}
