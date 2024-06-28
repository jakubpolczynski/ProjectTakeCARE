using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class ExaminationTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitId",
                table: "ExaminationTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationTable_VisitId",
                table: "ExaminationTable",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTable_VisitTable_VisitId",
                table: "ExaminationTable",
                column: "VisitId",
                principalTable: "VisitTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTable_VisitTable_VisitId",
                table: "ExaminationTable");

            migrationBuilder.DropIndex(
                name: "IX_ExaminationTable_VisitId",
                table: "ExaminationTable");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "ExaminationTable");
        }
    }
}
