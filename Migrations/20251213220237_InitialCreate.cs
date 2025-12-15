using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices");

            migrationBuilder.DropIndex(
                name: "IX_TrainerServices_TrainerId",
                table: "TrainerServices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TrainerServices");

            migrationBuilder.AddColumn<int>(
                name: "WorkEndHour",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkStartHour",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices",
                columns: new[] { "TrainerId", "ServiceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices");

            migrationBuilder.DropColumn(
                name: "WorkEndHour",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "WorkStartHour",
                table: "Trainers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TrainerServices",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerServices_TrainerId",
                table: "TrainerServices",
                column: "TrainerId");
        }
    }
}
