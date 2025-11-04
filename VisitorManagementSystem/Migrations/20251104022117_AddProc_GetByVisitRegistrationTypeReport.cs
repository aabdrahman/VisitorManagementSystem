using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddProc_GetByVisitRegistrationTypeReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[ProcGetByVisitRegistrationTypeReport]
                    @StartDate DATE,
                    @EndDate DATE

                AS

                BEGIN

                    SET NOCOUNT ON; 

                    SELECT VisitorRegistrationType AS [VisitorRegistrationType], COUNT(*) AS [NumberOfRecords]
                    FROM dbo.VisitDetails
                    WHERE VisitationDate >= CAST(@StartDate AS date) AND VisitationDate <= CAST(@EndDate AS date)
                    GROUP BY VisitorRegistrationType

                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[ProcGetByVisitRegistrationTypeReport]");
        }
    }
}
