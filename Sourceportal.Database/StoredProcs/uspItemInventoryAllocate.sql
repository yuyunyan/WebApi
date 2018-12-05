/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.05.18
   Description:	Sets qty of old item inventory line, and adds new item inventory line created in SAP
   Usage: EXEC uspItemInventoryAllocate
   Revision History:
		2018.06.22	NA	**DEPRECATED after ItemStock schema change**
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspItemInventoryAllocate]
    @RemainingQty INT,
	@NewQty INT,
	@OldStockID INT,
	@NewStockExternalID NVARCHAR(32),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewItemInventoryID INT

	INSERT INTO ItemInventory(POLineID, ItemID, InvStatusID, WarehouseBinID, Qty, ReceivedDate, DateCode, PackagingID, PackageConditionID, ExternalID, CreatedBy, IsDeleted)
	SELECT POLineID, ItemID, InvStatusID, WarehouseBinID, @NewQty, ReceivedDate, DateCode, PackagingID, PackageConditionID, @NewStockExternalID, @UserID, 0  
	FROM ItemInventory 
	WHERE InventoryID = @OldStockID

	SET @NewItemInventoryID = @@IDENTITY

	IF @@ROWCOUNT = 0
		RETURN -4
	
	UPDATE ItemInventory
	SET
		Qty = @RemainingQty
	WHERE InventoryID = @OldStockID

	SELECT @NewItemInventoryID 'InventoryID'
END