/* =============================================
	Author:		Berry
	Create date: 2017.08.04
	Description: Inserts or updates a Purchase Order Extra on a Purchase Order
	Return Codes:
			-15  @PurchaseOrderID and @POVersionID are both required to create a new record
			-16  The POVersionID provided is not the latest POVersionID for the given PurchaseOrderID
			-17	Error on Insert
			-18  Purchase Order Extras that are not on the latest version of the Purchase Order cannot be updated
			-19	Error on Update
			-6  @UserID is required
			-20  @ItemExtraID is required
	Revision History:

	============================================= */
CREATE PROCEDURE [dbo].[uspPurchaseOrderExtraSet] 
	@POExtraID INT = NULL,
	@PurchaseOrderID INT = NULL,
	@POVersionID INT = NULL,	
	@ItemExtraID INT = NULL,
	@RefLineNum INT = NULL,
	@StatusID INT = NULL,
	@Qty INT = NULL,	
	@Cost MONEY = NULL,
	@PrintOnPO BIT = 0,
	@Note NVARCHAR(250) = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -6
	IF ISNULL(@ItemExtraID, 0) = 0
		RETURN -20
		
	IF ISNULL(@POExtraID, 0) = 0
		BEGIN
			IF ISNULL(@PurchaseOrderID, 0) = 0 OR ISNULL(@POVersionID, 0) = 0
				RETURN -15
			--Check to make sure the version number given is the latest for the SalesOrder
			DECLARE @LatestVersion INT = (SELECT ISNULL(VersionID, -5) FROM vwPurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID)
			IF @POVersionID <> @LatestVersion
				RETURN -16
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine

InsertLine:
	--Get the next LineNum
	DECLARE @LineNum INT = NULL
	
	SET @LineNum = 
		(SELECT ISNULL(MAX(LineNum), 0) + 1 
		FROM PurchaseOrderExtras
		WHERE PurchaseOrderID = @PurchaseOrderID AND POVersionID = @POVersionID)
	
	--Get the default status
	SET @StatusID = (SELECT TOP 1 ISNULL(StatusID, 0)
					FROM lkpStatuses 
					WHERE IsDeleted = 0 
					  AND IsDefault = 1 
					  AND ObjectTypeID = 18)  --ID of the SalesOrder Extra object type

	--Create the record
	INSERT INTO PurchaseOrderExtras (PurchaseOrderID, POVersionID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Cost, PrintOnPO, Note, CreatedBy)
	VALUES (@PurchaseOrderID,
			@POVersionID,
			@StatusID,
			@ItemExtraID,
			@LineNum,
			@RefLineNum,
			@Qty,
			@Cost,
			@PrintOnPO,
			@Note,
			@UserID) --CreatedBy
			
	SET @POExtraID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -17
	GOTO ReturnSelect

UpdateLine:	
	--Get the SalesOrderID and VersionID of the line to be updated
	SELECT  @POVersionID = POVersionID, 
			@PurchaseOrderID = PurchaseOrderID 
	FROM PurchaseOrderExtras
	WHERE POExtraID = @POExtraID
	
	--If the line is not on the most recent version, return an error
	DECLARE @LatestVersionUpdate INT = (SELECT ISNULL(VersionID, -5) FROM vwPurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID)
	IF @POVersionID <> @LatestVersionUpdate
		RETURN -18

	--Update the record
	UPDATE PurchaseOrderExtras
	SET	
		StatusID = @StatusID,
		ItemExtraID = @ItemExtraID,
		RefLineNum = @RefLineNum,
		Qty = @Qty,
		Cost = @Cost,
		PrintOnPO = @PrintOnPO,
		Note = @Note,		
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE POExtraID = @POExtraID

	IF (@@ROWCOUNT=0)
		RETURN -19
	GOTO ReturnSelect

ReturnSelect:
	SELECT @POExtraID 'POExtraID',
		@LineNum 'LineNum'
END
