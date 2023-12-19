using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressTable",
                columns: table => new
                {
                    Address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_postal_code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTable", x => x.Address_id);
                });

            migrationBuilder.CreateTable(
                name: "UserTable",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTable", x => x.User_id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorTable",
                columns: table => new
                {
                    Doctor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Doctor_first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doctor_last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doctor_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doctor_phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doctor_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorTable", x => x.Doctor_id);
                    table.ForeignKey(
                        name: "FK_DoctorTable_UserTable_User_id",
                        column: x => x.User_id,
                        principalTable: "UserTable",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientTable",
                columns: table => new
                {
                    Patient_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patient_pesel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientTable", x => x.Patient_id);
                    table.ForeignKey(
                        name: "FK_PatientTable_AddressTable_Address_id",
                        column: x => x.Address_id,
                        principalTable: "AddressTable",
                        principalColumn: "Address_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientTable_UserTable_UserId",
                        column: x => x.UserId,
                        principalTable: "UserTable",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationTable",
                columns: table => new
                {
                    Examination_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Examination_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Examination_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Examination_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Examination_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Examination_result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_id = table.Column<int>(type: "int", nullable: false),
                    Doctor_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationTable", x => x.Examination_id);
                    table.ForeignKey(
                        name: "FK_ExaminationTable_DoctorTable_Doctor_id",
                        column: x => x.Doctor_id,
                        principalTable: "DoctorTable",
                        principalColumn: "Doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationTable_PatientTable_Patient_id",
                        column: x => x.Patient_id,
                        principalTable: "PatientTable",
                        principalColumn: "Patient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitTable",
                columns: table => new
                {
                    Visit_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patient_id = table.Column<int>(type: "int", nullable: false),
                    Doctor_id = table.Column<int>(type: "int", nullable: false),
                    Visit_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visit_description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTable", x => x.Visit_id);
                    table.ForeignKey(
                        name: "FK_VisitTable_DoctorTable_Doctor_id",
                        column: x => x.Doctor_id,
                        principalTable: "DoctorTable",
                        principalColumn: "Doctor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitTable_PatientTable_Patient_id",
                        column: x => x.Patient_id,
                        principalTable: "PatientTable",
                        principalColumn: "Patient_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTable_User_id",
                table: "DoctorTable",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationTable_Doctor_id",
                table: "ExaminationTable",
                column: "Doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationTable_Patient_id",
                table: "ExaminationTable",
                column: "Patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_PatientTable_Address_id",
                table: "PatientTable",
                column: "Address_id");

            migrationBuilder.CreateIndex(
                name: "IX_PatientTable_UserId",
                table: "PatientTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTable_Doctor_id",
                table: "VisitTable",
                column: "Doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTable_Patient_id",
                table: "VisitTable",
                column: "Patient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExaminationTable");

            migrationBuilder.DropTable(
                name: "VisitTable");

            migrationBuilder.DropTable(
                name: "DoctorTable");

            migrationBuilder.DropTable(
                name: "PatientTable");

            migrationBuilder.DropTable(
                name: "AddressTable");

            migrationBuilder.DropTable(
                name: "UserTable");
        }
    }
}
