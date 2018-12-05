-- =============================================
-- Author:				Berry, Zhong
-- Create date:			11.10.2017
-- Description:			Insert or update record for user navigation role
-- =============================================
CREATE PROCEDURE [dbo].[uspNavigationRoleForUserSet]
	@RoleID INT = NULL,
	@UserID INT = NULL,
	@UserRoleID INT = NULL,
	@IsDeleted BIT = 0,
	@Creator INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@UserRoleID, 0) = 0 
		GOTO InsertRecord
	ELSE 
		GOTO UpdateRecord

UpdateRecord:
	UPDATE mapUserRoles 
	SET IsDeleted = @IsDeleted
	WHERE UserRoleID = @UserRoleID
   
	GOTO ReturnSelect;

InsertRecord:
	INSERT INTO mapUserRoles (UserID, RoleID, FilterObjectTypeID, FilterTypeID, FilterObjectID, CreatedBy, IsDeleted)
	VALUES (@UserID, @RoleID, 0, 0, 0, @Creator, @IsDeleted)

	SET @UserRoleID = SCOPE_IDENTITY();

	GOTO ReturnSelect;
ReturnSelect:
	SELECT 
		R.RoleID,
		R.RoleName,
		UR.UserRoleID,
		UR.IsDeleted
	FROM Roles R
		LEFT OUTER JOIN mapUserRoles UR ON UR.RoleID = R.RoleID AND UR.UserID = @UserID
	WHERE ObjectTypeID = 8 AND UR.UserRoleID = @UserRoleID
END