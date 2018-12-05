/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.17
   Description:	Retrieves user roles list
   Usage: EXEC uspUserRolesGet

   Revision:
	2017.05.30	AR	Added column FilterObjectID and FilterObjectTypeID
	2017.10.25	BZ	Added FilterTypeID and ObjectName
   ============================================= */
CREATE PROCEDURE [dbo].[uspUserRolesGet]
(
	@UserID INT = NULL,
	@UserRoleID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT R.[RoleID],
		[RoleName],
		UR.FilterObjectID,
		UR.FilterObjectTypeID,
		UR.FilterTypeID,
		R.[ObjectTypeID],
		OT.ObjectName,
		s.TypeDescription,
		s.TypeSecurityID,
		UR.UserRoleID
	FROM Roles R
	INNER JOIN mapUserRoles UR on UR.RoleID = R.RoleID
	INNER JOIN lkpObjectTypes OT on OT.ObjectTypeID = R.ObjectTypeID
	INNER JOIN lkpObjectTypeSecurity s on s.ObjectTypeID = OT.ObjectTypeID AND s.FilterTypeID = UR.FilterTypeID AND s.FilterObjectTypeID = UR.FilterObjectTypeID
	WHERE UR.UserID = @UserID AND UR.IsDeleted = 0 AND UR.UserRoleID = ISNULL(@UserRoleID, UR.UserRoleID)
	ORDER BY UR.Created
END
