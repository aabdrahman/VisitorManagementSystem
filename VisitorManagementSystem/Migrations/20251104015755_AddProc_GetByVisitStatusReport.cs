using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddProc_GetByVisitStatusReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[ProcGetByVisitStatusReport]
                    @StartDate DATE,
                    @EndDate DATE

                AS

                BEGIN

                    SET NOCOUNT ON; 

                    SELECT VisitStatus, COUNT(*) AS [NumberOfRecords]
                    FROM dbo.VisitDetails
                    WHERE VisitationDate >= CAST(@StartDate AS date) AND VisitationDate <= CAST(@EndDate AS date)
                    GROUP BY VisitStatus

                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[ProcGetByVisitStatusReport]");
        }
    }
}
