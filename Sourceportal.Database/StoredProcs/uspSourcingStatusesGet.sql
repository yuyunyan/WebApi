/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Gets a list of Statuses used for Sourcing.  Excludes closed/canceled/complete statuses.  
				Includes an "IsDefault" boolean to denote this status should be selected by default on the Sources grid.
   Usage: EXEC uspSourcingStatusesGet
   Revision History:
   Return Codes:
   ============================================= */
   
CREATE PROCEDURE [dbo].[uspSourcingStatusesGet]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	StatusID, 
			StatusName, 
			CASE WHEN ConfigValue IS NOT NULL THEN 1 ELSE 0 END AS 'IsDefault'
	FROM lkpStatuses 
	  LEFT OUTER JOIN lkpConfigVariables ON StatusID = CAST(ConfigValue AS INT) AND ConfigName = 'QuoteLineRoutedToBuyerStatus'
	WHERE ObjectTypeID = 20  --QuoteLine object type
	  AND IsDeleted = 0
	  AND IsCanceled = 0
	  AND IsComplete = 0
END
