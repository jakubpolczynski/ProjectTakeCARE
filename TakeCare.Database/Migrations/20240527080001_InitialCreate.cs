using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "DoctorTable",
                newName: "Specialization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialization",
                table: "DoctorTable",
                newName: "Title");
        }
    }
}
