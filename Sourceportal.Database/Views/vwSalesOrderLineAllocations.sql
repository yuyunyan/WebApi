/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.03.28
   Description:	Shows the SOLineID with totals for amounts allocated from POs and Inventory.
				The PO total is reduced when the PO's inventory is received and allocated.
   Revision History:
				2018.05.10	NA	Added IsCanceled and IsDeleted checks on the PO lines and Inventory items
				2018.06.22	NA	Converted to ItemStock schema
				2018.08.13	NA	Added WarehouseID
   ============================================= */

CREATE OR ALTER VIEW [dbo].[vwSalesOrderLineAllocations]
AS
	SELECT	sol.SOLineID,
			sol.Qty 'OrderQty',
			ISNULL(sopo.Qty, 0) - ISNULL(consumed.Qty, 0) 'POAllocated',
			ISNULL(soinv.Qty, 0) 'InvAllocated',
			ISNULL(sopo.WarehouseID, soinv.WarehouseID) 'WarehouseID'
	FROM vwSalesOrderLines sol
		--The inventory allocated to the SO line
		LEFT OUTER JOIN (	SELECT SOLineID, SUM(soinv.Qty) 'Qty', MAX(WarehouseID) 'WarehouseID'
							FROM mapSOInvFulfillment soinv
							  INNER JOIN vwStockQty sq ON soinv.StockID = sq.StockID AND sq.IsDeleted = 0
							WHERE soinv.IsDeleted = 0 
							GROUP BY SOLineID) soinv ON sol.SOLineID = soinv.SOLineID
		--The purchase order lines allocated to the SO line
		LEFT OUTER JOIN (	SELECT SOLineID, SUM(sopo.Qty) 'Qty', MAX(WarehouseID) 'WarehouseID'
							FROM mapSOPOAllocation sopo
							  INNER JOIN vwPurchaseOrderLines pol ON sopo.POLineID = pol.POLineID
							  INNER JOIN vwPurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID
							  INNER JOIN lkpStatuses spol ON pol.StatusID = spol.StatusID
							  INNER JOIN lkpStatuses spo ON po.StatusID = spo.StatusID
							  LEFT OUTER JOIN Warehouses w ON po.ToWarehouseID = w.WarehouseID
							WHERE sopo.IsDeleted = 0 
							  AND spol.IsCanceled = 0
							  AND spo.IsCanceled = 0
							GROUP BY SOLineID) sopo ON sol.SOLineID = sopo.SOLineID 
		--The inventory that matches a PO line
		LEFT OUTER JOIN (	SELECT sopo.SOLineID, SUM(ful.Qty) 'Qty'
							FROM mapSOPOAllocation sopo
							  INNER JOIN vwPurchaseOrderLines pol ON sopo.POLineID = pol.POLineID
							  INNER JOIN lkpStatuses s ON pol.StatusID = s.StatusID
							  INNER JOIN vwStockQty sq ON sopo.POLineID = sq.POLineID AND sq.IsDeleted = 0
							  INNER JOIN mapSOInvFulfillment ful ON sq.StockID = ful.StockID AND sopo.SOLineID = ful.SOLineID AND ful.IsDeleted = 0
							WHERE sopo.IsDeleted = 0
							  AND s.IsCanceled = 0
							GROUP BY sopo.SOLineID) consumed ON sol.SOLineID = consumed.SOLineID