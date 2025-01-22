﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mini_project_csharp.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdClient",
                table: "Clientes",
                newName: "IdClientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdClientes",
                table: "Clientes",
                newName: "IdClient");
        }
    }
}
