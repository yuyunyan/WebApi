/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates external id from sap sync
   Usage:	EXEC [uspItemSapDataSet] @ItemID = 1, @ExternalId = '345', @UserID = 1		
   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspItemSapDataSet]
	@ItemID INT,
	@ExternalId VARCHAR(50),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE Items
	SET		
		ExternalId = @ExternalId,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE ItemID = @ItemID 
END