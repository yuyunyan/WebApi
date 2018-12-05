/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.12
   Description:	Retrieves all fields by RoleID

   Revision History:
   2017.11.14	BZ	Added CanEdit, Join with mapRoleFieldPermission table
   2017.12.11	BZ	Added FieldType
   ============================================= */
CREATE PROCEDURE [dbo].[uspFieldsByRoleGet]
(
	@RoleID INT = 0
)
AS
BEGIN
	SELECT R.RoleID
	, R.ObjectTypeID
	, R.IsDeleted
	, R.RoleID
	, RFP.CanEdit
	, F.FieldID
	, F.FieldName
	, F.FieldType
	FROM Roles R
	INNER JOIN mapRoleFieldPermissions RFP on RFP.RoleID = R.RoleID
	INNER JOIN lkpFields F on F.FieldID = RFP.FieldID
	WHERE R.RoleID = @RoleID
END
