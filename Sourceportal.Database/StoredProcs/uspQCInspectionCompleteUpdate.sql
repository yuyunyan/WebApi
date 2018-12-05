/* =============================================
   Author:		Julia Thomas
   Create date: 2017.09.17
   Description:	Inserts CompletedDate and CompletedBy per inspectionId on inpection tbl 
   Usage:		EXEC uspQCInspectionCompleteUpdate 
   Revision History:
     
     Return Codes:
			
   ============================================= */
ALTER PROCEDURE [dbo].[uspQCInspectionCompleteUpdate]
(
	@InspectionID INT= NULL,
	@CreatedBy INT = NULL
)
AS
BEGIN
	UPDATE QCInspections
	SET CompletedDate= GETUTCDATE(), CompletedBy= @CreatedBy
	WHERE InspectionID = @InspectionID
	SELECT @@ROWCOUNT 'ROWCOUNT'	
END