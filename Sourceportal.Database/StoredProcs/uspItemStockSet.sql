/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.06.25
   Description:	Inserts or updates a record in the ItemStock table
   Usage: 
   Revision History:
   2018.07.03	AR	Added NULLIF to update statement
   2018.07.10	NA	Reverted back to working procedure
						-Removed warehouseBinID.  That does not belong here.  Use uspItemInventorySet.
						-Removed creation of a 0 qty ItemInventory
						-Removed update of ALL iteminventory records for the stock to set WarehouseBinID
   2018.08.15   CT  Added ClonedFromID to insert of ItemStock
   2018.11.01	NA	Added MfrLotNum
   Return Codes:
		-1	Error creating new stock record

   ============================================= */
CREATE OR ALTER PROCEDURE [dbo].[uspItemStockSet]
(
	@StockID INT = NULL,
	@POLineID INT = NULL,
	@ItemID INT = NULL,
	@InvStatusID INT = NULL,
	@IsRejected BIT = NULL,	
	@ReceivedDate DATETIME = NULL,
	@DateCode VARCHAR(50) = NULL,
	@PackagingID INT = NULL,
	@PackageConditionID INT = NULL,	
	@COO INT = NULL,
	@Expiry DATE = NULL,
	@StockDescription VARCHAR(200) = NULL,
	@MfrLotNum VARCHAR(50) = NULL,
	@ExternalID VARCHAR(50) = NULL,
	@IsDeleted BIT = NULL,
	@ClonedFromID INT = NULL,
	@UserID INT = NULL	
)
AS
BEGIN
	DECLARE @ObjectTypeID INT = 107 --ItemStock object type ID
	SET NOCOUNT ON;
	IF (ISNULL(@StockID, 0) = 0)
		GOTO InsertStock
	ELSE 
		GOTO UpdateStock

InsertStock:
	INSERT INTO ItemStock (POLineID, ItemID, InvStatusID, IsRejected, ReceivedDate, DateCode, PackagingID, PackageConditionID, COO, Expiry, StockDescription, MfrLotNum, ExternalID, CreatedBy, ClonedFromID)
	VALUES (	@POLineID,
				@ItemID,
				@InvStatusID,
				ISNULL(@IsRejected, 0),	
				ISNULL(@ReceivedDate, GETUTCDATE()),
				@DateCode,
				@PackagingID,
				@PackageConditionID,
				@COO,
				@Expiry,
				@StockDescription,
				@MfrLotNum,
				@ExternalID,				
				@UserID,
				@ClonedFromID
		)
	SET @StockID = SCOPE_IDENTITY()

	IF (ISNULL(@StockID,0) = 0)
		RETURN -1

	--Create a Source for the newly created Inventory
	DECLARE @SourceID INT = NULL
	DECLARE @SourceTypeID INT = (SELECT SourceTypeID FROM lkpSourceTypes WHERE TypeName = 'Inventory')
	
	INSERT INTO Sources (SourceTypeID, ItemID, CommodityID, AccountID, ContactID, CurrencyID, PartNumber, PartNumberStrip, Manufacturer, Qty, Cost, DateCode, PackagingID, PackageConditionID, CreatedBy)
	SELECT	@SourceTypeID, @ItemID, i.CommodityID, po.AccountID, po.ContactID, po.CurrencyID, i.PartNumber, i.PartNumberStrip, m.MfrName, 0, pol.Cost, ISNULL(@DateCode, pol.DateCode), ISNULL(@PackagingID, pol.PackagingID), ISNULL(@PackageConditionID, pol.PackageConditionID), @UserID
	FROM Items i
	  INNER JOIN PurchaseOrderLines pol ON i.ItemID = pol.ItemID AND pol.POLineID = @POLineID
	  INNER JOIN PurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID
	  INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	WHERE i.ItemID = @ItemID

	SET @SourceID = SCOPE_IDENTITY()
	
	--Join the Source and the Inventory
	INSERT INTO mapSourcesJoin (ObjectTypeID, ObjectID, SourceID, CreatedBy)
	VALUES (@ObjectTypeID, @StockID, @SourceID, @UserID)

	GOTO SelectOutput

DeleteStock:
	--Delete the stock and its inventory and source

UpdateStock:
	--Update the ItemStock record
	DECLARE @CurrentIsRejected BIT 
	SELECT @CurrentIsRejected = IsRejected FROM ItemStock WHERE StockID = @StockID
	
	UPDATE ItemStock
	SET POLineID = ISNULL(NULLIF(@POLineID,0), POLineID),
		ItemID = ISNULL(NULLIF(@ItemID,0), ItemID),
		InvStatusID = ISNULL(NULLIF(@InvStatusID,0), InvStatusID),
		IsRejected = ISNULL(@IsRejected, IsRejected),
		ReceivedDate = ISNULL(@ReceivedDate, ReceivedDate),
		DateCode = ISNULL(@DateCode, DateCode),
		PackagingID = ISNULL(@PackagingID, PackagingID),	
		PackageConditionID = ISNULL(@PackageConditionID, PackageConditionID),	
		COO = ISNULL(@COO, COO),
		Expiry = ISNULL(@Expiry, Expiry),
		StockDescription = ISNULL(@StockDescription, StockDescription),
		MfrLotNum = ISNULL(@MfrLotNum, MfrLotNum),
		ExternalID = ISNULL(NULLIF(@ExternalID,''), ExternalID),	
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,		
		Modified = GETUTCDATE()  
	WHERE StockID = @StockID
	
	--Update the related Source
	UPDATE s
	SET s.ItemID = ISNULL(@ItemID, s.ItemID),
		s.PartNumber = i.PartNumber,
		s.PartNumberStrip = i.PartNumberStrip,
		s.Manufacturer = m.MfrName,
		s.CommodityID = i.CommodityID,		
		s.DateCode = ISNULL(@DateCode, s.DateCode),
		s.PackagingID = ISNULL(@PackagingID, s.PackagingID),	
		s.PackageConditionID = ISNULL(@PackageConditionID, s.PackageConditionID),	
		--Delete the source if the stock is either deleted or rejected
		s.IsDeleted = ISNULL(CASE WHEN @IsDeleted = 1 OR @IsRejected = 1 THEN 1 
								  WHEN @IsDeleted = 0 AND ISNULL(@IsRejected, @CurrentIsRejected) = 0 THEN 0
								  ELSE NULL END, 
							 s.IsDeleted),	
		s.ModifiedBy = @UserID,
		s.Modified = GETUTCDATE()
	FROM Sources s
	  INNER JOIN mapSourcesJoin sj ON s.SourceID = sj.SourceID
		AND sj.ObjectID = @StockID
		AND sj.ObjectTypeID = @ObjectTypeID
		AND sj.IsDeleted = 0	  
	  INNER JOIN Items i ON ISNULL(@ItemID, s.ItemID) = i.ItemID
	  INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	  
	GOTO SelectOutput
	
SelectOutput:
	SELECT @StockID 'StockID'	
END
