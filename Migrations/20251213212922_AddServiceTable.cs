using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonu.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03d49aa7-6a52-4110-a707-699a7475a5cf", "AQAAAAIAAYagAAAAEB7F6Ztf/5ddcccsQ3lokDW2IrOWDhs71HJYi8JlJN6059ExyB8/NEhHaHbwlw6YuA==", "fb71cca7-b0a2-40bf-adb2-0301668044e4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Services");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71724eae-c099-46ab-9a4b-ec9e84612c83", "AQAAAAIAAYagAAAAEIzbh3pdSYeuRHoxmLF/TMpsN0GxRrnTqbHExwwy5E2uQuZTZqAb/AwRPkpmQsiqkQ==", "68d55361-8954-4368-a59f-00875a0b2057" });
        }
    }
}
