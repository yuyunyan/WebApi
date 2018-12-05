/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.12
   Description:	Retrieves all navigation records by RoleID

   Revision History:
   2017.11.15	BZ		Join with mapRoleNavPermissions table
   ============================================= */
CREATE PROCEDURE [dbo].[uspNavigationByRoleGet]
(
	@RoleID INT = 0
)
AS
BEGIN
	SELECT 
		N.ParentNavID,
		N.NavName, 
		N.NavID,
		R.RoleID,
		MRP.IsDeleted
	FROM Roles R
		INNER JOIN mapRoleNavPermissions MRP ON MRP.RoleID = R.RoleID
		INNER JOIN lkpNavigation N ON N.NavID = MRP.NavID AND N.IsNavMenu = 1
	WHERE ObjectTypeID = 8 AND R.RoleID = @RoleID

END
