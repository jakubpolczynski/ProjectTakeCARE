using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakeCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class NamingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTable_UserTable_User_id",
                table: "DoctorTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTable_DoctorTable_Doctor_id",
                table: "ExaminationTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTable_PatientTable_Patient_id",
                table: "ExaminationTable");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTable_AddressTable_Address_id",
                table: "PatientTable");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTable_DoctorTable_Doctor_id",
                table: "VisitTable");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTable_PatientTable_Patient_id",
                table: "VisitTable");

            migrationBuilder.RenameColumn(
                name: "Visit_description",
                table: "VisitTable",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Visit_date",
                table: "VisitTable",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Patient_id",
                table: "VisitTable",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "Doctor_id",
                table: "VisitTable",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "Visit_id",
                table: "VisitTable",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_VisitTable_Patient_id",
                table: "VisitTable",
                newName: "IX_VisitTable_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitTable_Doctor_id",
                table: "VisitTable",
                newName: "IX_VisitTable_DoctorId");

            migrationBuilder.RenameColumn(
                name: "User_password",
                table: "UserTable",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "User_email",
                table: "UserTable",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "User_Role",
                table: "UserTable",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "UserTable",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Patient_phone",
                table: "PatientTable",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Patient_pesel",
                table: "PatientTable",
                newName: "Pesel");

            migrationBuilder.RenameColumn(
                name: "Patient_last_name",
                table: "PatientTable",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Patient_first_name",
                table: "PatientTable",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Patient_email",
                table: "PatientTable",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Address_id",
                table: "PatientTable",
                newName: "AddressId");

            migrationBuilder.RenameColumn(
                name: "Patient_id",
                table: "PatientTable",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTable_Address_id",
                table: "PatientTable",
                newName: "IX_PatientTable_AddressId");

            migrationBuilder.RenameColumn(
                name: "Patient_id",
                table: "ExaminationTable",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "Examination_type",
                table: "ExaminationTable",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Examination_result",
                table: "ExaminationTable",
                newName: "Result");

            migrationBuilder.RenameColumn(
                name: "Examination_name",
                table: "ExaminationTable",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Examination_description",
                table: "ExaminationTable",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Examination_date",
                table: "ExaminationTable",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Doctor_id",
                table: "ExaminationTable",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "Examination_id",
                table: "ExaminationTable",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationTable_Patient_id",
                table: "ExaminationTable",
                newName: "IX_ExaminationTable_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationTable_Doctor_id",
                table: "ExaminationTable",
                newName: "IX_ExaminationTable_DoctorId");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "DoctorTable",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Doctor_title",
                table: "DoctorTable",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Doctor_phone",
                table: "DoctorTable",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Doctor_last_name",
                table: "DoctorTable",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Doctor_first_name",
                table: "DoctorTable",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Doctor_email",
                table: "DoctorTable",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Doctor_id",
                table: "DoctorTable",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorTable_User_id",
                table: "DoctorTable",
                newName: "IX_DoctorTable_UserId");

            migrationBuilder.RenameColumn(
                name: "Address_street",
                table: "AddressTable",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Address_postal_code",
                table: "AddressTable",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "Address_city",
                table: "AddressTable",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "Address_id",
                table: "AddressTable",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorTable_UserTable_UserId",
                table: "DoctorTable",
                column: "UserId",
                principalTable: "UserTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTable_DoctorTable_DoctorId",
                table: "ExaminationTable",
                column: "DoctorId",
                principalTable: "DoctorTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTable_PatientTable_PatientId",
                table: "ExaminationTable",
                column: "PatientId",
                principalTable: "PatientTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTable_AddressTable_AddressId",
                table: "PatientTable",
                column: "AddressId",
                principalTable: "AddressTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTable_DoctorTable_DoctorId",
                table: "VisitTable",
                column: "DoctorId",
                principalTable: "DoctorTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTable_PatientTable_PatientId",
                table: "VisitTable",
                column: "PatientId",
                principalTable: "PatientTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTable_UserTable_UserId",
                table: "DoctorTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTable_DoctorTable_DoctorId",
                table: "ExaminationTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationTable_PatientTable_PatientId",
                table: "ExaminationTable");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTable_AddressTable_AddressId",
                table: "PatientTable");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTable_DoctorTable_DoctorId",
                table: "VisitTable");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTable_PatientTable_PatientId",
                table: "VisitTable");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "VisitTable",
                newName: "Patient_id");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "VisitTable",
                newName: "Doctor_id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "VisitTable",
                newName: "Visit_description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "VisitTable",
                newName: "Visit_date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VisitTable",
                newName: "Visit_id");

            migrationBuilder.RenameIndex(
                name: "IX_VisitTable_PatientId",
                table: "VisitTable",
                newName: "IX_VisitTable_Patient_id");

            migrationBuilder.RenameIndex(
                name: "IX_VisitTable_DoctorId",
                table: "VisitTable",
                newName: "IX_VisitTable_Doctor_id");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "UserTable",
                newName: "User_password");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UserTable",
                newName: "User_email");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "UserTable",
                newName: "User_Role");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserTable",
                newName: "User_id");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "PatientTable",
                newName: "Patient_phone");

            migrationBuilder.RenameColumn(
                name: "Pesel",
                table: "PatientTable",
                newName: "Patient_pesel");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "PatientTable",
                newName: "Patient_last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "PatientTable",
                newName: "Patient_first_name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "PatientTable",
                newName: "Patient_email");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "PatientTable",
                newName: "Address_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PatientTable",
                newName: "Patient_id");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTable_AddressId",
                table: "PatientTable",
                newName: "IX_PatientTable_Address_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ExaminationTable",
                newName: "Examination_type");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "ExaminationTable",
                newName: "Examination_result");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "ExaminationTable",
                newName: "Patient_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ExaminationTable",
                newName: "Examination_name");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "ExaminationTable",
                newName: "Doctor_id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ExaminationTable",
                newName: "Examination_description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ExaminationTable",
                newName: "Examination_date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ExaminationTable",
                newName: "Examination_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationTable_PatientId",
                table: "ExaminationTable",
                newName: "IX_ExaminationTable_Patient_id");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationTable_DoctorId",
                table: "ExaminationTable",
                newName: "IX_ExaminationTable_Doctor_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DoctorTable",
                newName: "User_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "DoctorTable",
                newName: "Doctor_title");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "DoctorTable",
                newName: "Doctor_phone");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "DoctorTable",
                newName: "Doctor_last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "DoctorTable",
                newName: "Doctor_first_name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "DoctorTable",
                newName: "Doctor_email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DoctorTable",
                newName: "Doctor_id");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorTable_UserId",
                table: "DoctorTable",
                newName: "IX_DoctorTable_User_id");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "AddressTable",
                newName: "Address_street");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "AddressTable",
                newName: "Address_postal_code");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "AddressTable",
                newName: "Address_city");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AddressTable",
                newName: "Address_id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorTable_UserTable_User_id",
                table: "DoctorTable",
                column: "User_id",
                principalTable: "UserTable",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTable_DoctorTable_Doctor_id",
                table: "ExaminationTable",
                column: "Doctor_id",
                principalTable: "DoctorTable",
                principalColumn: "Doctor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationTable_PatientTable_Patient_id",
                table: "ExaminationTable",
                column: "Patient_id",
                principalTable: "PatientTable",
                principalColumn: "Patient_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTable_AddressTable_Address_id",
                table: "PatientTable",
                column: "Address_id",
                principalTable: "AddressTable",
                principalColumn: "Address_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTable_DoctorTable_Doctor_id",
                table: "VisitTable",
                column: "Doctor_id",
                principalTable: "DoctorTable",
                principalColumn: "Doctor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTable_PatientTable_Patient_id",
                table: "VisitTable",
                column: "Patient_id",
                principalTable: "PatientTable",
                principalColumn: "Patient_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
