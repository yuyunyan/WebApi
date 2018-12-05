/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.10.08
   Description:	Retrieves all Warehouse SOS Details for a SOLineID
   Usage:		SELECT * FROM dbo.tfnGetWarehouseSoSDetails(68)
   Revision History:
		2018.10.08	AR	Initial deployment

   ============================================= */
CREATE FUNCTION [dbo].[tfnGetWarehouseSoSDetails]
(
	@SOLineID INT
)
RETURNS TABLE
AS
RETURN (
	--Inventory Allocations
	SELECT wh.OrganizationID, wh.ShipFromRegionID
	FROM  mapSOInvFulfillment soinv
	INNER JOIN vwStockQty sq ON soinv.StockID = sq.StockID AND sq.IsDeleted = 0
	INNER JOIN Warehouses wh ON sq.WarehouseID = wh.WarehouseID
	WHERE soinv.IsDeleted = 0 AND soinv.SOLineID = @SOLineID

	UNION
	--Purchase Order Allocations
	SELECT wh.OrganizationID, wh.ShipFromRegionID
	FROM mapSOPOAllocation sopo
	INNER JOIN vwPurchaseOrderLines pol ON sopo.POLineID = pol.POLineID
	INNER JOIN vwPurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID
	INNER JOIN Warehouses wh ON po.ToWarehouseID = wh.WarehouseID
	WHERE sopo.IsDeleted = 0 AND sopo.SOLineID = @SOLineID
)