/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.11
   Description:	Retrieves all permissions from lkpPermissions

   Revision History:
   2017.11.14	BZ	Rename column Name to PermName
   ============================================= */
CREATE PROCEDURE [dbo].[uspPermissionsGet]
(
	@ObjectTypeID INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	ObjectTypeID,
	PermName,
	PermissionID,
	PermDescription 'Description'
	FROM lkpPermissions
	WHERE ObjectTypeID = @ObjectTypeID
	AND IsDeleted = 0
END
