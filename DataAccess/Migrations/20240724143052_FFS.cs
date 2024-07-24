using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FFS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileItems_Folders_FolderId",
                table: "FileItems");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "FileItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "FileItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "FileItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_FileItems_Folders_FolderId",
                table: "FileItems",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileItems_Folders_FolderId",
                table: "FileItems");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "FileItems");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "FileItems");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "FileItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileItems_Folders_FolderId",
                table: "FileItems",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
