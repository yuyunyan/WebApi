/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.05.21
   Description:	Gets Item Inventory as well as several attributes of its StockID
   Usage: EXEC uspItemInventoryGet @InventoryID = 110
   Revision History:
		2018.06.22	NA	Converted to ItemStock schema
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspItemInventoryGet]
(
	@InventoryID INT
)
AS
BEGIN
	SELECT
		ii.InventoryID,
		ist.POLineID,
		ist.ItemID,
		ist.InvStatusID,
		ii.WarehouseBinID,
		WB.BinName,
		WB.ExternalID 'WarehouseBinExternalId',
		WB.ExternalUUID 'WarehouseBinExternalUUID', 
		W.WarehouseID,
		W.ExternalID 'WarehouseExternalId',
		ii.Qty,
		ist.ReceivedDate,
		ist.DateCode,
		ist.PackagingID,
		ist.PackageConditionID,
		ist.ExternalID,
		ii.IsDeleted,
		ist.IsDeleted 'StockDeleted'
	FROM ItemInventory ii
	INNER JOIN ItemStock ist ON ii.StockID = ist.StockID
	INNER JOIN WarehouseBins WB on WB.WarehouseBinID = ii.WarehouseBinID
	INNER JOIN Warehouses W on W.WarehouseID = WB.WarehouseID
	WHERE InventoryID = @InventoryID
END