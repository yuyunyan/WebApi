/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.03
   Description:	Inserts or updates a line item on a Purchase Order
   Usage:	EXEC uspPurchaseOrderLineSet @POLineID = 12345 [...]
			EXEC uspPurchaseOrderLineSet @PurchaseOrderID = 100002, @POVersionID = 2 [...]			
   Return Codes:
			-8 Missing PurchaseOrderID or POVersionID for new record
			-9 Error inserting new Purchase order line
			-10 ItemID is required
			-11 Error updating record
			-12 POVersionID is not the latest version for the given SalesOrderID
			-13 Line items on old versions of a Purchase order cannot be updated
			-6 Missing UserID
   Revision History:
			2018.03.08	NA	Added Package Condition
			2018.05.10	NA	Added update for SOPOAllocation records
			2018.05.17	BZ	Added ClonedFromID
			2018.05.24	NA	Added LineRev
			2018.06.19  CT  Added Parameters for LineNum and LineRev setting for Insert
			2018.06.20	NA	Modified LineNumManual and LineRevManual parameters
			2018.06.21  CT  Added StatusID and VendorLine to check ISNULL parameters on Update
			2018.07.13	NA	Added ToWarehouseID
   ============================================= */

CREATE PROCEDURE [dbo].[uspPurchaseOrderLineSet]
	@POLineID INT = NULL,
	@PurchaseOrderID INT = NULL,
	@POVersionID INT = NULL,	
	@StatusID INT = NULL,
	@ItemID INT = NULL,	
	@VendorLine INT = NULL,	
	@Qty INT = NULL,		
	@Cost MONEY = NULL,	
	@DateCode NVARCHAR(25) = NULL,
	@PackagingID INT = NULL,
	@PackageConditionID INT = NULL,
	@ToWarehouseID INT = NULL,
	@DueDate DATE = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL,
	@IsSpecBuy BIT = NULL,
	@SpecBuyForUserID INT = NULL,
	@SpecBuyForAccountID INT = NULL,
	@SpecBuyReason NVARCHAR(200) = NULL,
	@ClonedFromID INT = NULL,
	@LineNum INT = NULL,
	@LineRev INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@ItemID, 0) = 0
		RETURN -10

	IF @UserID IS NULL
		RETURN -6
	
	IF ISNULL(@POLineID, 0) = 0
		BEGIN
			IF ISNULL(@PurchaseOrderID, 0) = 0 OR ISNULL(@POVersionID, 0) = 0
				RETURN -8
			--Check to make sure the version number given is the latest for the Purchase order
			DECLARE @LatestVersion INT = (SELECT ISNULL(VersionID, -5) FROM vwPurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID)
			IF @POVersionID <> @LatestVersion
				RETURN -12
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine
	
InsertLine:
	/*
	DECLARE @LineNum INT = NULL		
	DECLARE @LineRev INT = 0
	*/
	IF @ClonedFromID IS NOT NULL AND @LineRev IS NULL
	BEGIN
	--Get the next LineRev if line is cloned
		SET @LineNum = (SELECT LineNum FROM PurchaseOrderLines WHERE POLineID = @ClonedFromID)		
		SET @LineRev =
			(SELECT ISNULL(MAX(LineRev), 0) + 1
			FROM PurchaseOrderLines
			WHERE PurchaseOrderID = @PurchaseOrderID AND POVersionID = @POVersionID AND LineNum = @LineNum)
	END
	ELSE IF @LineNum IS NULL
	BEGIN
	--Get the next LineNum
		SET @LineNum = 
			(SELECT ISNULL(MAX(LineNum), 0) + 1 
			FROM PurchaseOrderLines 
			WHERE PurchaseOrderID = @PurchaseOrderID AND POVersionID = @POVersionID)
	END

	--Get the default status
	SET @StatusID = (SELECT TOP 1 ISNULL(StatusID, 0)
					FROM lkpStatuses 
					WHERE IsDeleted = 0 
					  AND IsDefault = 1 
					  AND ObjectTypeID = 23)  --ID of the Purchase Order Line object type

	--Create the record
	INSERT INTO PurchaseOrderLines (PurchaseOrderID, POVersionID, StatusID, ItemID, LineNum, LineRev, VendorLine, Qty, Cost, DateCode, PackagingID, PackageConditionID, ToWarehouseID, DueDate, IsSpecBuy, SpecBuyForAccountID, SpecBuyForUserID, SpecBuyReason, CreatedBy, ClonedFromID)
	VALUES (@PurchaseOrderID, 
			@POVersionID, 			
			@StatusID, 
			@ItemID,
			@LineNum,
			ISNULL(@LineRev, 0),
			CASE WHEN ISNULL(@VendorLine,0) = 0 THEN @LineNum ELSE @VendorLine END, --VendorLine
			@Qty,			
			@Cost, 			
			@DateCode, 
			@PackagingID,
			@PackageConditionID,
			@ToWarehouseID,
			@DueDate,
			@IsSpecBuy, 	
			@SpecBuyForAccountID,
			@SpecBuyForUserID,
			@SpecBuyReason	,	
			@UserID, --CreatedBy
			@ClonedFromID)
			
	SET @POLineID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -9
	GOTO ReturnSelect

UpdateLine:	
	--Get the PurchaseOrderID and VersionID of the line to be updated
	SELECT  @POVersionID = POVersionID, 
			@PurchaseOrderID = PurchaseOrderID 
	FROM PurchaseOrderLines 
	WHERE POLineID = @POLineID
	
	--If the line is not on the most recent version, return an error
	DECLARE @LatestVersionUpdate INT = (SELECT ISNULL(VersionID, -5) FROM vwPurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID)
	IF @POVersionID <> @LatestVersionUpdate
		RETURN -13
	ELSE
		--Update the record
		UPDATE PurchaseOrderLines
		SET	
			StatusID = ISNULL(@StatusID, StatusID),
			ItemID = @ItemID,		
			VendorLine = ISNULL(@VendorLine, VendorLine),		
			Qty = @Qty,		
			Cost = @Cost,		
			DateCode = @DateCode,
			PackagingID = @PackagingID,
			PackageConditionID = @PackageConditionID,
			ToWarehouseID = ISNULL(@ToWarehouseID, ToWarehouseID),
			DueDate = @DueDate,	
			IsDeleted = ISNULL(@IsDeleted, IsDeleted),
			IsSpecBuy = ISNULL(@IsSpecbuy, IsSpecBuy),
			SpecBuyForUserID = @SpecBuyForUserID,
			SpecBuyForAccountID = @SpecBuyForAccountID,
			SpecBuyReason = @SpecBuyReason,
			ModifiedBy = @UserID,
			Modified = GETUTCDATE()
		WHERE POLineID = @POLineID	
	
	IF (@@ROWCOUNT=0)
		RETURN -11
	
	--Update the qty on Allocations since the entire line must be allocated to a SO
	UPDATE mapSOPOAllocation
	SET Qty = @Qty,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE POLineID = @POLineID
	AND IsDeleted = 0
	
	GOTO ReturnSelect

ReturnSelect:
	SELECT @POLineID 'POLineID'
END
