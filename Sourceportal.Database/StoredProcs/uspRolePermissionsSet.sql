/* =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.11.16
-- Description:			Insert or Update mapRolePermissions table
-- =============================================*/
CREATE PROCEDURE [dbo].[uspRolePermissionsSet]
	@ListData VARCHAR(MAX) = NULL,
	@RoleID INT = NULL,
	@CreatorID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE mapRolePermissions
	WHERE RoleID = @RoleID AND IsDeleted = 0

	INSERT INTO mapRolePermissions (RoleID, PermissionID, CreatedBy, IsDeleted)
	SELECT 
		@RoleID,
		P.PermissionID,
		@CreatorID,
		0
	FROM
	OPENJSON(@ListData)
		WITH(
			PermissionID INT) P
			
	DECLARE @UpdateRowCount INT = @@ROWCOUNT

	SELECT @UpdateRowCount 'RowCount'
END
