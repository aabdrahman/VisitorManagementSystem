using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUserDetails_Proc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            
                CREATE OR ALTER PROCEDURE [dbo].[GetUserSummaryDetails]
	                    @NumberOfRecords INT,
	                    @NumberOfRecordsToSkip INT,
	                    @RoleName NVARCHAR(50),
	                    @Username NVARCHAR(100) = ''
                  AS

                BEGIN
	                SET NOCOUNT ON;
		                SELECT  [u].[StaffId] AS [StaffId],
                                [u].[FirstName] AS [FirstName], 
                                [u].[LastName] AS [LastName], 
                                [u].[Email] AS [Email], 
                                [u].[PhoneNumber] AS [PhoneNumber], 
                                [u].[isActive] AS [Status], 
                                [r].[Name] AS [Role], 
                                [u].[CreatedAt] AS [DateJoined]
		                FROM dbo.AspNetUsers u
		                JOIN dbo.AspNetUserRoles ur 
		                ON [u].[Id] = [ur].[UserId]
		                INNER JOIN dbo.AspNetRoles r
		                ON [r].[Id] = [ur].[RoleId]
                        WHERE 
                        (
                            (@RoleName = '' OR r.Name LIKE '%' + @RoleName + '%')
                            AND (@Username = '' OR u.UserName LIKE '%' + @Username + '%')
                        )
		                ORDER BY u.CreatedAt
		                OFFSET @NumberOfRecordsToSkip ROWS
		                FETCH NEXT @NumberOfRecords ROWS ONLY
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetUserSummaryDetails]");
        }
    }
}
