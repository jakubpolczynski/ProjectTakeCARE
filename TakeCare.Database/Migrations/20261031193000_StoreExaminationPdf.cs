using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TakeCare.Database.Data;

#nullable disable

namespace TakeCare.Database.Migrations;

[DbContext(typeof(TakeCareDbContext))]
[Migration("20261031193000_StoreExaminationPdf")]
public partial class StoreExaminationPdf : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Fail without disclosing records or deleting data when legacy duplicates exist.
        migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [ExaminationTable] GROUP BY [VisitId] HAVING COUNT_BIG(*) > 1) THROW 50001, 'Duplicate examinations per visit require manual review.', 1;");
        migrationBuilder.AddColumn<byte[]>(name: "PdfContent", table: "ExaminationTable",
            type: "varbinary(max)", nullable: true);
        migrationBuilder.DropIndex(name: "IX_ExaminationTable_VisitId", table: "ExaminationTable");
        migrationBuilder.CreateIndex(name: "IX_ExaminationTable_VisitId", table: "ExaminationTable",
            column: "VisitId", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "IX_ExaminationTable_VisitId", table: "ExaminationTable");
        migrationBuilder.CreateIndex(name: "IX_ExaminationTable_VisitId", table: "ExaminationTable", column: "VisitId");
        migrationBuilder.DropColumn(name: "PdfContent", table: "ExaminationTable");
    }
}
