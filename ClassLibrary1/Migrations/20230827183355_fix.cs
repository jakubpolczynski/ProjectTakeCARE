using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTable_User_User_id",
                table: "DoctorTable");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTable_User_UserId",
                table: "PatientTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorTable_UserTable_User_id",
                table: "DoctorTable",
                column: "User_id",
                principalTable: "UserTable",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTable_UserTable_UserId",
                table: "PatientTable",
                column: "UserId",
                principalTable: "UserTable",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTable_UserTable_User_id",
                table: "DoctorTable");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTable_UserTable_UserId",
                table: "PatientTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable");

            migrationBuilder.RenameTable(
                name: "UserTable",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorTable_User_User_id",
                table: "DoctorTable",
                column: "User_id",
                principalTable: "User",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTable_User_UserId",
                table: "PatientTable",
                column: "UserId",
                principalTable: "User",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
