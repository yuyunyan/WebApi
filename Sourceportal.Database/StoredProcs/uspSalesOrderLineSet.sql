/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Inserts or updates a line item on a Sales Order
   Usage:	EXEC uspSalesOrderLineSet @SOLineID = 12345 [...]
			EXEC uspSalesOrderLineSet @SalesOrderID = 100002, @SOVersionID = 2 [...]			
   Return Codes:
			-1 Missing SalesOrderID or SOVersionID for new record
			-2 Error inserting new sales order line
			-3 ItemID is required
			-4 Error updating record
			-5 SOVersionID is not the latest version for the given SalesOrderID
			-6 Line items on old versions of a sales order cannot be updated
			-7 Missing UserID
   Revision History:
			2018.03.08	NA	Added PackageConditionID
			2018.06.12	NA	Added @LineNum for create from SAP
			2018.07.30	NA	Added @ProcWarehouseID
			2018.08.09	NA	Removed @ProcWarehouseID, Added DeliveryStatus and InvoiceStatus
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderLineSet]
	@SOLineID INT = NULL,
	@SalesOrderID INT = NULL,
	@SOVersionID INT = NULL,
	@QuoteLineID INT = NULL,
	@StatusID INT = NULL,
	@ItemID INT = NULL,	
	@LineNum INT = NULL,
	@CustomerLine INT = NULL,
	@CustomerPartNum NVARCHAR(32) = NULL,	
	@Qty INT = NULL,	
	@Price MONEY = NULL,
	@Cost MONEY = NULL,	
	@DateCode NVARCHAR(25) = NULL,
	@PackagingID INT = NULL,
	@PackageConditionID INT = NULL,
	@DeliveryRuleID INT = NULL,	
	@DeliveryStatus VARCHAR(100) = NULL,
	@InvoiceStatus VARCHAR(100) = NULL,
	@ShipDate DATE = NULL,
	@DueDate DATE = NULL,	
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@ItemID, 0) = 0
		RETURN -3

	IF @UserID IS NULL
		RETURN -7

	--Get known ItemID information	
	DECLARE @PartNumberStrip NVARCHAR(32) = NULL
	
	SELECT
		@PartNumberStrip = i.PartNumberStrip
	FROM Items i
	WHERE ItemID = @ItemID
		
	
	IF ISNULL(@SOLineID, 0) = 0
		BEGIN
			IF ISNULL(@SalesOrderID, 0) = 0 OR ISNULL(@SOVersionID, 0) = 0
				RETURN -1
			--Check to make sure the version number given is the latest for the sales order
			DECLARE @LatestVersion INT = (SELECT ISNULL(VersionID, -5) FROM vwSalesOrders WHERE SalesOrderID = @SalesOrderID)
			IF @SOVersionID <> @LatestVersion
				RETURN -5
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine
	
InsertLine:
	--Get the next LineNum if it wasn't defined.
	IF @LineNum IS NULL
	BEGIN
	SET @LineNum = 
		(SELECT ISNULL(MAX(LineNum), 0) + 1 
		FROM SalesOrderLines 
		WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @SOVersionID)
	END

	--Get the default status
	SET @StatusID = (SELECT TOP 1 ISNULL(StatusID, 0)
					FROM lkpStatuses 
					WHERE IsDeleted = 0 
					  AND IsDefault = 1 
					  AND ObjectTypeID = 17)  --ID of the Sales Order Line object type
	
	IF @DeliveryRuleID IS NULL
		SET @DeliveryRuleID = (SELECT TOP 1 DeliveryRuleID FROM SalesOrders WHERE SalesOrderID = @SalesOrderID AND VersionID = @SOVersionID)

	--Create the record
	INSERT INTO SalesOrderLines (SalesOrderID, SOVersionID, QuoteLineID, StatusID, ItemID, LineNum, CustomerLine, CustomerPartNum, PartNumberStrip, Qty, Price, Cost, DateCode, PackagingID, PackageConditionID, DeliveryRuleID, ShipDate, DueDate, DeliveryStatus, InvoiceStatus, CreatedBy)
	VALUES (@SalesOrderID, 
			@SOVersionID, 
			NULLIF(@QuoteLineID, 0), 
			@StatusID, 
			@ItemID,			
			@LineNum,
			CASE WHEN ISNULL(@CustomerLine,0) = 0 THEN @LineNum ELSE @CustomerLine END, --CustomerLine
			@CustomerPartNum, 			
			@PartNumberStrip, 			
			@Qty, 			
			@Price, 
			@Cost, 			
			@DateCode, 
			@PackagingID, 
			@PackageConditionID,
			@DeliveryRuleID,			
			@ShipDate, 
			@DueDate,
			@DeliveryStatus,
			@InvoiceStatus,
			@UserID) --CreatedBy
			
	SET @SOLineID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -2
	GOTO ReturnSelect

UpdateLine:	
	--Get the SalesOrderID and VersionID of the line to be updated
	SELECT  @SOVersionID = SOVersionID, 
			@SalesOrderID = SalesOrderID 
	FROM SalesOrderLines 
	WHERE SOLineID = @SOLineID
	
	--If the line is not on the most recent version, return an error
	DECLARE @LatestVersionUpdate INT = (SELECT ISNULL(VersionID, -5) FROM vwSalesOrders WHERE SalesOrderID = @SalesOrderID)
	IF @SOVersionID <> @LatestVersionUpdate
		RETURN -6

	--Update the record
	UPDATE SalesOrderLines
	SET	
		StatusID = (CASE WHEN @StatusID = 0 THEN StatusID ELSE @StatusID END),
		ItemID = @ItemID,		
		CustomerLine = @CustomerLine,
		CustomerPartNum = @CustomerPartNum,		
		PartNumberStrip = @PartNumberStrip,		
		Qty = @Qty,		
		Price = @Price,
		Cost = @Cost,		
		DateCode = @DateCode,
		PackagingID = @PackagingID,
		PackageConditionID = @PackageConditionID,
		DeliveryRuleID = ISNULL(@DeliveryRuleID, DeliveryRuleID),		
		ShipDate = @ShipDate,
		DueDate = @DueDate,	
		DeliveryStatus = ISNULL(@DeliveryStatus, DeliveryStatus),
		InvoiceStatus = ISNULL(@InvoiceStatus, InvoiceStatus),
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE SOLineID = @SOLineID

	IF (@@ROWCOUNT=0)
		RETURN -4
	GOTO ReturnSelect

ReturnSelect:
	SELECT @SOLineID 'SOLineID'
END
