/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.03.07
   Description:	Inserts or updates a record in the ItemInventory table, as well as the related Source record
   Usage: 
   Revision History:
       2018.05.21	CT  Updated the Update portions to check for NULL case so that if a parameter is passed in as null it will not overwrite the existing value
	   2018.06.25	NA	Replaced with new ItemStock schema

   Return Codes:
		-1	Error creating new inventory record
		-2	StockID is required for insert
   ============================================= */
CREATE PROCEDURE [dbo].[uspItemInventorySet]
(
	@InventoryID INT = NULL,
	@StockID INT = NULL,
	@WarehouseBinID INT = NULL,
	@Qty INT = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL	
)
AS
BEGIN
	DECLARE @ObjectTypeID INT = 107 --ItemInventory object type ID
	SET NOCOUNT ON;
	IF (ISNULL(@InventoryID, 0) = 0)
		GOTO InsertInventory
	ELSE
		GOTO UpdateInventory

InsertInventory:
	IF @StockID IS NULL
		RETURN -2
	
	INSERT INTO ItemInventory (StockID, WarehouseBinID, Qty, CreatedBy)
	VALUES					  (@StockID, @WarehouseBinID, @Qty, @UserID)
	
	SET @InventoryID = SCOPE_IDENTITY()

	IF (ISNULL(@InventoryID,0) = 0)
		RETURN -1

	GOTO UpdateSource

UpdateInventory:
	--Update the ItemInventory record
	UPDATE ItemInventory
	SET WarehouseBinID = ISNULL(@WarehouseBinID, WarehouseBinID),
		Qty = ISNULL(@Qty, Qty),
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),	
		ModifiedBy = @UserID,		
		Modified = GETUTCDATE()  
	WHERE InventoryID = @InventoryID

	GOTO UpdateSource

UpdateSource:	
	--Update the related Source
	DECLARE @TotalQty INT
	SELECT @TotalQty = SUM(Qty)
	FROM ItemInventory
	WHERE StockID = @StockID
	  AND IsDeleted = 0
	
	UPDATE s
	SET s.Qty = ISNULL(@TotalQty, 0),
		s.ModifiedBy = @UserID,
		s.Modified = GETUTCDATE()
	FROM Sources s
	  INNER JOIN mapSourcesJoin sj ON s.SourceID = sj.SourceID
		AND sj.ObjectID = @StockID
		AND sj.ObjectTypeID = @ObjectTypeID
		AND sj.IsDeleted = 0

	GOTO SelectOutput
	
SelectOutput:
	SELECT @InventoryID 'InventoryID'	
END
