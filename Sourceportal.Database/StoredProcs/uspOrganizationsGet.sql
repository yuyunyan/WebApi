/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.11.02
   Description:	Gets a list of orgs
   Usage: EXEC [uspOrganizationsGet]
   Revision History:
       07.23.2018	NA	Added ObjectTypeID
   Return Codes:
   ============================================= */

ALTER PROCEDURE [dbo].[uspOrganizationsGet]
	@OrganizationID int = 0,
	@ObjectTypeID int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		o.OrganizationID,
		o.ParentOrgID,
		o.[Name],
		o.ExternalID
	FROM Organizations o
	LEFT OUTER JOIN mapOrgObjectTypes mot ON o.OrganizationID = mot.OrganizationID AND mot.ObjectTypeID = @ObjectTypeID
	WHERE o.OrganizationID = ISNULL(NULLIF(@OrganizationID, 0), o.OrganizationID)
		AND (mot.ObjectTypeID IS NOT NULL OR @ObjectTypeID IS NULL)
END