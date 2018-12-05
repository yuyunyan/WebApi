/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.10.30
   Description:	Returns a list of navigation and permissions for a given UserID
   Usage:	EXEC uspUserGeneralSecurityGet @UserID = 64
			
   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspUserGeneralSecurityGet]
	@UserID INT = NULL
AS
BEGIN
	SELECT 'Nav' 'Type', rnp.NavID 'ID', n.NavName 'Name'
	FROM mapRoleNavPermissions rnp
	  INNER JOIN Roles r ON rnp.RoleID = r.RoleID AND r.ObjectTypeID = 8 AND r.IsDeleted = 0
	  INNER JOIN mapUserRoles ur ON r.RoleID = ur.RoleID AND ur.UserID = @UserID AND ur.IsDeleted = 0
	  INNER JOIN lkpNavigation n ON rnp.NavID = n.NavID AND n.IsDeleted = 0 AND n.IsNavMenu = 1
	WHERE rnp.IsDeleted = 0 

	UNION

	SELECT 'Action' 'Type', p.ObjectTypeID, p.PermName 'Name'
	FROM mapRolePermissions rp
	  INNER JOIN Roles r ON rp.RoleID = r.RoleID AND r.IsDeleted = 0
	  INNER JOIN mapUserRoles ur ON r.RoleID = ur.RoleID AND ur.UserID = @UserID AND ur.IsDeleted = 0
	  INNER JOIN lkpPermissions p ON rp.PermissionID = p.PermissionID AND p.IsDeleted = 0 AND p.IsObjectSpecific = 0
	WHERE rp.IsDeleted = 0
END
