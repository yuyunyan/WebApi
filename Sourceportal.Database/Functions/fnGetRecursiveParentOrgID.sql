/* =============================================
	   Author:		Aaron Rodecker
	   Created:		2018.06.18
	   Description:	Gets the recursive parent ID from organizations
	   Usage:		SELECT dbo.fnGetRecursiveParentOrgID(6)
					SELECT * FROM ORganizations WHERE P
	   Revision History:
       
	   Return Codes:
   ============================================= */
CREATE FUNCTION [dbo].[fnGetRecursiveParentOrgID]
(
	@OrganizationID INT
)
RETURNS INT
AS
BEGIN
	DECLARE @ParentOrganizationID INT = NULL, @NewOrgID INT = NULL

--SelectParent:
--	SELECT @ParentOrganizationID = ISNULL(ParentOrgID, 0)
--		, @NewOrgID = OrganizationID
--	FROM Organizations
--	WHERE OrganizationID = ISNULL(@ParentOrganizationID, @OrganizationID)

--	IF (ISNULL(@ParentOrganizationID,0) != 0)
--		GOTO SelectParent
--	RETURN @NewOrgID

	WHILE (ISNULL(ISNULL(@ParentOrganizationID, @OrganizationID),0) != 0)
	BEGIN
		SELECT @ParentOrganizationID = ISNULL(ParentOrgID, 0)
		, @NewOrgID = OrganizationID
		FROM Organizations
		WHERE OrganizationID = ISNULL(@ParentOrganizationID, @OrganizationID )
	END
	RETURN @NewOrgID
END
