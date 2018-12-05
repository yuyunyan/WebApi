/* =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.11.17
-- Description:			Insert or Update mapRoleNavPermissions table
-- =============================================*/
CREATE PROCEDURE [dbo].[uspRoleNavLinksSet]
	@ListData VARCHAR(MAX) = NULL,
	@RoleID INT = NULL,
	@CreatorID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE mapRoleNavPermissions
	WHERE RoleID = @RoleID AND IsDeleted = 0

	INSERT INTO mapRoleNavPermissions (RoleID, NavID, CreatedBy, IsDeleted)
	SELECT 
		@RoleID,
		NL.NavID,
		@CreatorID,
		0
	FROM
	OPENJSON(@ListData)
		WITH(
			NavID INT) NL
			
	DECLARE @UpdateRowCount INT = @@ROWCOUNT

	SELECT @UpdateRowCount 'RowCount'
END
