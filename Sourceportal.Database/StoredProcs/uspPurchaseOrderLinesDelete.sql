
/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.08.08
   Description:	Deletes one or more Purchase Order Lines
   Usage:	EXEC uspPurchaseOrderLinesDelete @POLinesJSON = '[{"POLineID":16}, {"POLineID":88}]', @UserID = 0		
   Return Codes:
			-14 Missing JSON list of Purchase Order Lines to be deleted
			-6 Missing UserID
   Revision History:
			
   ============================================= */

   
CREATE PROCEDURE [dbo].[uspPurchaseOrderLinesDelete]
	@POLinesJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@ResultCount INT = NULL OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@POLinesJSON, '') = ''
		RETURN -14

	IF ISNULL(@UserID, 0) = 0
		RETURN -6

	UPDATE pol
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM PurchaseOrderLines pol
	  INNER JOIN OPENJSON(@POLinesJSON) WITH (POLineID INT) AS j ON pol.POLineID = j.POLineID

	SET @ResultCount = @@ROWCOUNT

	SELECT @ResultCount 'ResultCount'
END
GO