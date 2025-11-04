using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRepoortProcColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[ProcGetByVisitRegistrationTypeReport]
                    @StartDate DATE,
                    @EndDate DATE

                AS

                BEGIN

                    SET NOCOUNT ON; 

                    SELECT VisitorRegistrationType AS [ReportFilter], COUNT(*) AS [NumberOfRecords]
                    FROM dbo.VisitDetails
                    WHERE VisitationDate >= CAST(@StartDate AS date) AND VisitationDate <= CAST(@EndDate AS date)
                    GROUP BY VisitorRegistrationType

                END
            ");

            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[ProcGetByVisitStatusReport]
                    @StartDate DATE,
                    @EndDate DATE

                AS

                BEGIN

                    SET NOCOUNT ON; 

                    SELECT VisitStatus AS [ReportFilter], COUNT(*) AS [NumberOfRecords]
                    FROM dbo.VisitDetails
                    WHERE VisitationDate >= CAST(@StartDate AS date) AND VisitationDate <= CAST(@EndDate AS date)
                    GROUP BY VisitStatus

                END
            ");

            migrationBuilder.Sql(@"
                 ALTER PROCEDURE [dbo].[ProcGetByVisitTypeReport]
                    @StartDate DATE,
                    @EndDate DATE

                AS

                BEGIN

                    SET NOCOUNT ON; 

                    SELECT VisitType AS [ReportFilter], COUNT(*) AS [NumberOfRecords]
                    FROM dbo.VisitDetails
                    WHERE VisitationDate >= CAST(@StartDate AS date) AND VisitationDate <= CAST(@EndDate AS date)
                    GROUP BY VisitType
    

                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[ProcGetByVisitRegistrationTypeReport]
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

            migrationBuilder.Sql(@"
                CREATE OR ALTER   PROCEDURE [dbo].[ProcGetByVisitStatusReport]
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

            migrationBuilder.Sql(@"
                 CREATE OR ALTER   PROCEDURE [dbo].[ProcGetByVisitTypeReport]
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
    }
}
