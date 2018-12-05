/* =============================================
   Author:		Julia Thomas
   Create date: 2018.06.28
   Description:	 update QCInspections ResultID PER InspectionID
   Usage:	EXEC uspQCInspectionResultUpdate @InspectionID=2,@ResultID=2
   Return Codes:
			
   Revision History:
			2018.07.24	NA	Fixed ModifedBy
   ============================================= */

ALTER PROCEDURE [dbo].[uspQCInspectionResultUpdate]
    @InspectionID INT = NULL,
	@ResultID INT =  NULL,	
	@UserID INT = NULL
AS
BEGIN
	UPDATE QCInspections SET ResultID = @ResultID,ModifiedBy = @UserID, Modified = GETUTCDATE() WHERE InspectionID= @InspectionID
	SELECT @@ROWCOUNT 'ROWCOUNT'
END
