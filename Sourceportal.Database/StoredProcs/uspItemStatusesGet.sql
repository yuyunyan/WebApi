/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.26
   Description:	Gets a list of item statuses
   Usage: EXEC uspItemStatusesGet @ItemStatusID = 1
   Revision History:
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspItemStatusesGet]
	@ItemStatusID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		ItemStatusID,
		StatusName
	FROM lkpItemStatuses
	WHERE IsDeleted = 0
	  AND ItemStatusID = ISNULL(@ItemStatusID, ItemStatusID)
	ORDER BY StatusName
END
