using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class ExaminationTableChange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExaminationImageTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExaminationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationImageTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationImageTable_ExaminationTable_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "ExaminationTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationImageTable_ExaminationId",
                table: "ExaminationImageTable",
                column: "ExaminationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExaminationImageTable");
        }
    }
}
