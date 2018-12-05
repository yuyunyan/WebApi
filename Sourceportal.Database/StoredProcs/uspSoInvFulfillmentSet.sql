/* =============================================
   Author:		Julia Thomas 
   Create date: 2017.08.09
   Description:	Sets mapSOInvFulfillment or IsDeleted for a given SoLineID and StockID,Or insert new record to mapSOInvFulfillment
   Usage: EXEC sOInvFulfillmentSet
   Revision History:
		2018.06.26	NA	Updated to ItemStock schema
   Return Codes:
			-1 Missing UserID
			-2 Missing SoLineID
			-3 Missing StockID
			-4 Error in update sales-order & inventory mapping record
			-5 Error in insert sales-order & inventory mapping record
	Revision History
		2018.08.09	NA	Removed CreatedBy from update statement
   ============================================= */


CREATE OR ALTER PROCEDURE [dbo].[uspSoInvFulfillmentSet]
    @SOLineID INT = NULL,
	@StockID INT = NULL,
	@Qty INT=NULL,
	@ExternalID NVARCHAR(32) = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	IF ISNULL(@SOLineID, 0) = 0
		RETURN -2
	IF ISNULL(@StockID, 0) = 0
		RETURN -3		
  
  DECLARE @PreviousQty INT = (SELECT Qty FROM mapSOInvFulfillment WHERE SOLineID = @SOLineID AND StockID = @StockID)
	IF ISNULL(@PreviousQty, -1) = -1
		GOTO InsertSoInvFulfillment
	ELSE
		GOTO UpdateSoInvFulfillment


InsertSoInvFulfillment:
   INSERT INTO mapSOInvFulfillment (SOLineID, StockID, Qty, CreatedBy,ExternalID)
	VALUES (@SOLineID,
			@StockID,
			@Qty,
			@UserID,
			@ExternalID)
	GOTO ReturnSelect

 UpdateSoInvFulfillment:
	
	UPDATE mapSOInvFulfillment
	SET
		Qty = @Qty,
		ExternalID = @ExternalID,
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE SOLineID=@SOLineID AND StockID = @StockID

	IF (@@ROWCOUNT=0)
		RETURN -4

	GOTO ReturnSelect

ReturnSelect:
	SELECT @SOLineID 'SOLineID', @StockID 'StockID'
END