using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddProc_GetByVisitTypeReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[ProcGetByVisitTypeReport]
                    @StartDate DATE,
                    @EndDate DATE

                AS

                BEGIN

                    SET NOCOUNT ON; 

                    SELECT VisitType AS [VisitType], COUNT(*) AS [NumberOfRecords]
                    FROM dbo.VisitDetails
                    WHERE VisitationDate >= CAST(@StartDate AS date) AND VisitationDate <= CAST(@EndDate AS date)
                    GROUP BY VisitType
    

                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[ProcGetByVisitTypeReport]");
        }
    }
}
