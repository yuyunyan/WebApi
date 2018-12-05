/* =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.11.16
-- Description:			Insert or Update mapRoleFieldPermissions table
-- =============================================*/
CREATE PROCEDURE [dbo].[uspRoleFieldsSet]
	@ListData VARCHAR(MAX) = NULL,
	@RoleID INT = NULL,
	@CreatorID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE mapRoleFieldPermissions
	WHERE RoleID = @RoleID AND IsDeleted = 0

	INSERT INTO mapRoleFieldPermissions (RoleID, FieldID, CanEdit, CreatedBy, IsDeleted)
	SELECT 
		@RoleID,
		F.FieldID,
		F.CanEdit,
		@CreatorID,
		0
	FROM
	OPENJSON(@ListData)
		WITH(
			FieldID INT,
			CanEdit BIT) F
			
	DECLARE @UpdateRowCount INT = @@ROWCOUNT

	SELECT @UpdateRowCount 'RowCount'
END
