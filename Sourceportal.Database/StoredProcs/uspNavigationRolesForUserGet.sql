-- =============================================
-- Author:				Berry, Zhong
-- Create date:			11.09.2017
-- Description:			Return the status roles of page access 
-- Usage:				EXEC uspNavigationRolesForUserGet @UserID = 68
-- =============================================
CREATE PROCEDURE [dbo].[uspNavigationRolesForUserGet]
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		R.RoleID,
		R.RoleName,
		UR.UserRoleID,
		UR.IsDeleted
	FROM Roles R
		LEFT OUTER JOIN mapUserRoles UR ON UR.RoleID = R.RoleID AND UR.UserID = @UserID
	Where ObjectTypeID = 8

END
