using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteListParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "NoteLists",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteLists_ParentId",
                table: "NoteLists",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLists_NoteLists_ParentId",
                table: "NoteLists",
                column: "ParentId",
                principalTable: "NoteLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteLists_NoteLists_ParentId",
                table: "NoteLists");

            migrationBuilder.DropIndex(
                name: "IX_NoteLists_ParentId",
                table: "NoteLists");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "NoteLists");
        }
    }
}
