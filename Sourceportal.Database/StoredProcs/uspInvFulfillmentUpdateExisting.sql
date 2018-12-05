/* =============================================
   Author:		Corey Tyrrell 
   Create date: 2018.05.22
   Description:	Sets mapSOInvFulfillment or IsDeleted for a given InventoryId
   Usage: EXEC uspInvFulfillmentUpdateExisting
   Revision History:

   Return Codes:
			-1 Missing UserID
			-3 Missing InventoryID			
   ============================================= */


CREATE PROCEDURE [dbo].[uspInvFulfillmentUpdateExisting]
	@StockID INT = NULL,
	@ExternalID VARCHAR(50) = NULL,
	@Qty INT=NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	/*	This will update all records tied to the StockID, regardless of sales order.  This will work when only one sales
		order is allocated, but should eventually be updated to handle specific allocations  */
	SET NOCOUNT ON;

	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	IF ISNULL(@StockID, 0) = 0
		RETURN -3	
	
	UPDATE mapSOInvFulfillment
	SET
		Qty = @Qty,
		ExternalID = @ExternalID,
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE StockID = @StockID	

	GOTO ReturnSelect

ReturnSelect:
	SELECT @StockID 'StockID'
END