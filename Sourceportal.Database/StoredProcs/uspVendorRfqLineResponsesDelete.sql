/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.18
   Description:	Deletes one or more Rfq line responses
   Usage:	EXEC uspVendorRfqLineResponsesDelete @RFQLineRsponsesJSON = '[{"SourceID":16}, {"SourceID":88}]', @UserID = 0 @RFQLineId = 1 @UserId = 3		
   Return Codes:
			-13 Missing JSON list of Sales Order Lines to be deleted
			-14 Missing RFQLineId 
			-3 Missing UserID 
   Revision History:
			
   ============================================= */
   
CREATE PROCEDURE [dbo].[uspVendorRfqLineResponsesDelete]
	@RFQLineRsponsesJSON VARCHAR(MAX) = NULL,
	@RFQLineId VARCHAR(MAX) = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@RFQLineRsponsesJSON, '') = ''
		RETURN -13
	
	IF ISNULL(@RFQLineId, '') = ''
		RETURN -14
	
	IF ISNULL(@UserID , 0) = 0
		RETURN -3


	UPDATE Sources
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM Sources S
	  INNER JOIN OPENJSON(@RFQLineRsponsesJSON) WITH (SourceID INT) AS j ON S.SourceID = j.SourceID 

	UPDATE Mapsourcesjoin
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM Mapsourcesjoin MS
	  INNER JOIN OPENJSON(@RFQLineRsponsesJSON) WITH (SourceID INT) AS j 
	  ON MS.SourceID = j.SourceID AND  MS.ObjectID = @RFQLineId AND MS.ObjectTypeID = 28

END
