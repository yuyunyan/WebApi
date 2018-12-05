/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.09
   Description:	Retrieves roles list or a specific role from tblRoles
   Usage: EXEC uspUserRolesGet

   Revision:
   2017.05.30	AR	Added Objectname to select statement
   2017.11.17	BZ	Added SearchString for filtering

   ============================================= */
CREATE PROCEDURE [dbo].[uspRolesGet]
(
	@RoleID INT = NULL,
	@SearchString NVARCHAR(100) = ''
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		R.RoleID,
		R.RoleName,
		OT.[ObjectTypeID],
		OT.ObjectName
	FROM Roles R
	INNER JOIN lkpObjectTypes OT on OT.ObjectTypeID = R.ObjectTypeID
	WHERE R.RoleID = ISNULL(@RoleID,R.RoleID)
	AND R.IsDeleted = 0
	AND (ISNULL(R.RoleName, '') + ISNULL(OT.ObjectName, '') LIKE '%' + ISNULL(@SearchString,'') + '%')
END
