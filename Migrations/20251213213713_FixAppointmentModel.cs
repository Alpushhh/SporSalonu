using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SporSalonu.Migrations
{
    /// <inheritdoc />
    public partial class FixAppointmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "a1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Appointments",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TrainerServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerServices_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerServices_ServiceId",
                table: "TrainerServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerServices_TrainerId",
                table: "TrainerServices",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerServices");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Appointments",
                newName: "AppointmentId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "Uye", "UYE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthYear", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Gender", "Height", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Weight" },
                values: new object[] { "a1", 0, null, "03d49aa7-6a52-4110-a707-699a7475a5cf", "g232210029@sakarya.edu.tr", true, "Admin Kullanıcı", null, null, false, null, "G231210029@SAKARYA.EDU.TR", "G231210029@SAKARYA.EDU.TR", "AQAAAAIAAYagAAAAEB7F6Ztf/5ddcccsQ3lokDW2IrOWDhs71HJYi8JlJN6059ExyB8/NEhHaHbwlw6YuA==", null, false, "fb71cca7-b0a2-40bf-adb2-0301668044e4", false, "g231210029@sakarya.edu.tr", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "a1" });
        }
    }
}
