/* =============================================
	Author:		Berry, Zhong
	Create date: 2017.08.09
	Description:	Update or insert quantity allocation between a Sales Order line
		and Purchase Order line
	Return Codes:
				-1 Missing SOLineID or POLineID
				-2 Missing UserID
  =============================================*/
CREATE PROCEDURE [dbo].[uspPurchaseOrderAllocationSet] 
	@SOLineID INT = NULL,
	@POLineID INT = NULL,
	@IsDeleted BIT = 0,
	@Qty INT = 0,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (ISNULL(@SOLineID, 0) = 0) OR (ISNULL(@POLineID, 0) = 0)
		RETURN -1

	IF ISNULL(@UserID, 0) = 0
		RETURN -2
	
	DECLARE @PreviousQty INT = (SELECT Qty FROM mapSOPOAllocation WHERE SOLineID = @SOLineID AND POLineID = @POLineID)

	IF ISNULL(@PreviousQty, -1) = -1
		GOTO InsertLine
	ELSE
		GOTO UpdateLine

InsertLine:
	INSERT INTO mapSOPOAllocation (SOLineID, POLineID, Qty, CreatedBy)
	VALUES (@SOLineID,
			@POLineID,
			@Qty,
			@UserID)

	IF @IsDeleted = 0
		UPDATE mapSOPOAllocation SET IsDeleted = 1 WHERE SOLineID != @SOLineID AND POLineID = @POLineID

	GOTO ReturnSelect

UpdateLine:
	UPDATE mapSOPOAllocation
	SET 
		Qty = @Qty,
		IsDeleted = @IsDeleted,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE SOLineID = @SOLineID AND POLineID = @POLineID

	IF @IsDeleted = 0
		UPDATE mapSOPOAllocation SET IsDeleted = 1 WHERE SOLineID != @SOLineID AND POLineID = @POLineID
	
	GOTO ReturnSelect

ReturnSelect:
	SELECT 
		@SOLineID 'SOLineID',
		@POLineID 'POLineID',
		@Qty 'Qty',
		P.PurchaseOrderID,
		P.POVersionID
	FROM PurchaseOrderLines P
	WHERE P.POLineID = @POLineID
END
