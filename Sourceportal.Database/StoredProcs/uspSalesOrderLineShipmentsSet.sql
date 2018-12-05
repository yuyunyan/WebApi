/* =============================================
	Author:		Corey Tyrrell
	Create date: 2018.05.31
	Description:	Update or insert sales order line shipment mapping
	Return Codes:
				-1 Missing SOLineID or ShipmentID
				-2 Missing UserID
  =============================================*/
CREATE PROCEDURE [dbo].[uspSalesOrderLineShipmentsSet] 
	@SOLineID INT = NULL,
	@ShipmentID INT = NULL,
	@IsDeleted BIT = 0,
	@Qty INT = 0,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (ISNULL(@SOLineID, 0) = 0) OR (ISNULL(@ShipmentID, 0) = 0)
		RETURN -1

	IF ISNULL(@UserID, 0) = 0
		RETURN -2
	
	DECLARE @PreviousQty INT = (SELECT Qty FROM mapSalesOrderLineShipments WHERE SOLineID = @SOLineID AND ShipmentID = @ShipmentID)

	IF ISNULL(@PreviousQty, -1) = -1
		GOTO InsertLine
	ELSE
		GOTO UpdateLine

InsertLine:
	INSERT INTO mapSalesOrderLineShipments (SOLineID, ShipmentID, Qty, CreatedBy)
	VALUES (@SOLineID,
			@ShipmentID,
			@Qty,
			@UserID)

	--IF @IsDeleted = 0
	--	UPDATE mapSOPOAllocation SET IsDeleted = 1 WHERE SOLineID != @SOLineID AND POLineID = @POLineID

	GOTO ReturnSelect

UpdateLine:
	UPDATE mapSalesOrderLineShipments
	SET 
		Qty = @Qty,
		IsDeleted = @IsDeleted,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE SOLineID = @SOLineID AND ShipmentID = @ShipmentID

	--IF @IsDeleted = 0
	--	UPDATE mapSOPOAllocation SET IsDeleted = 1 WHERE SOLineID != @SOLineID AND POLineID = @POLineID
	
	GOTO ReturnSelect

ReturnSelect:
	SELECT 
		@SOLineID 'SOLineID',
		@ShipmentID 'ShipmentID',
		@Qty 'Qty'
END