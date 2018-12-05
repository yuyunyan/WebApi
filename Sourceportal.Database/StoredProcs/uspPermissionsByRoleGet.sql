/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.12
   Description:	Retrieves all permissions by RoleID

   Revision History:
   2017.05.19	AR	Added ObjectID = 0 to avoid selecting other permissions
   2017.11.14	BZ	Added PermName, Join with lkpPermissions table
   ============================================= */
CREATE PROCEDURE [dbo].[uspPermissionsByRoleGet]
(
	@RoleID INT = 0
)
AS
BEGIN
	SELECT 
		R.RoleID,
		R.ObjectTypeID,
		RP.IsDeleted,
		R.RoleName,
		RP.PermissionID,
		P.PermName,
		P.PermDescription 'Description'
	FROM Roles R		--Dont need to join ObjectID to permission ID? Since they are enums
	INNER JOIN mapRolePermissions RP on RP.RoleID = R.RoleID
	INNER JOIN lkpPermissions P ON RP.PermissionID = P.PermissionID
	WHERE R.RoleID = @RoleID

END
