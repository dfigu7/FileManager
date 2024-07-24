using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class hakunamatata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_FolderId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "FileItems");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "FileItems",
                newName: "FilePath");

            migrationBuilder.RenameIndex(
                name: "IX_Files_FolderId",
                table: "FileItems",
                newName: "IX_FileItems_FolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileItems",
                table: "FileItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileItems_Folders_FolderId",
                table: "FileItems",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileItems_Folders_FolderId",
                table: "FileItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileItems",
                table: "FileItems");

            migrationBuilder.RenameTable(
                name: "FileItems",
                newName: "Files");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Files",
                newName: "Path");

            migrationBuilder.RenameIndex(
                name: "IX_FileItems_FolderId",
                table: "Files",
                newName: "IX_Files_FolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_FolderId",
                table: "Files",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
