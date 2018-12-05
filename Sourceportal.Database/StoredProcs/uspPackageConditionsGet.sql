/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	Returns a list of package conditions
   Usage: EXEC [uspPackageConditionsGet]
   Revision History:
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspPackageConditionsGet]

@PackageConditionID int = 0

AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT PackageConditionID, ConditionName, ExternalID
	FROM codes.lkpPackageConditions
	WHERE IsDeleted = 0 AND PackageConditionID = ISNULL(NULLIF(@PackageConditionID, 0), PackageConditionID)
	ORDER BY ConditionName
END