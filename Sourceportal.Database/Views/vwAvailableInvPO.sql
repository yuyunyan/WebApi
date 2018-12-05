/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.08
   Description:	Gets a list of Inventory and PO lines and their AVAILABLE quantities

   Revision Hsqory:
	2018.03.26	AR	Removed PromiseDate
	2018.05.25	NA	Added IsDeleted check to ItemInventory
	2018.06.22	NA	Converted to ItemStock schema
	2018.07.18	AR	Added support for externalID
	2018.08.10	NA	Added Warehouse
	2018.08.23	NA	Added InTransit flag
	2018.08.28	NA	Split from uspAvailableInvPOGet into a View
	2018.10.30  HR  Added isInspection boolean
   Return Codes:
   ============================================= */

CREATE OR ALTER VIEW [dbo].[vwAvailableInvPO] AS
	SELECT	'Inventory' 'Type',
			sq.StockID 'ID',
			sq.ItemID, 
			po.AccountID,
			sq.InvStatusID 'StatusID',
			s.StatusName,
			sq.Qty 'Qty', --Quantity in inventory
			sq.Qty - SUM(ISNULL(ful.Qty, 0)) 'Available', 
			pol.Cost,
			pol.DateCode,
			sq.PackagingID,
			sq.PackageConditionID,
			pol.PurchaseOrderID,
			pol.POVersionID,
			po.ExternalID 'POExternalID',
			pol.LineNum,
			pol.LineRev,
			dbo.fnGetObjectOwners(pol.PurchaseOrderID, 22) 'Buyers',
			CAST(sq.ReceivedDate AS DATE) 'ShipDate',
			w.WarehouseID,
			w.WarehouseName,
			w.ExternalID 'WarehouseExternalID',	
			sq.InTransit,
			sq.IsInspection
	FROM vwStockQty sq
	  LEFT OUTER JOIN mapSOInvFulfillment ful ON sq.StockID = ful.StockID AND ful.IsDeleted = 0
	  INNER JOIN lkpItemInvStatuses s ON sq.InvStatusID = s.InvStatusID
	  INNER JOIN PurchaseOrderLines pol ON sq.POLineID = pol.POLineID
	  INNER JOIN PurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID AND pol.POVersionID = po.VersionID
	  INNER JOIN Warehouses w ON sq.WarehouseID = w.WarehouseID
	WHERE sq.IsDeleted = 0
	GROUP BY sq.StockID, 
			 sq.ItemID, 
			 po.AccountID, 
			 sq.InvStatusID, 
			 s.StatusName, 
			 sq.Qty, 
			 pol.Cost, 
			 pol.DateCode, 
			 sq.PackagingID, 
			 sq.PackageConditionID, 
			 pol.PurchaseOrderID, 
			 pol.POVersionID, 
			 po.ExternalID, 
			 pol.LineNum, 
			 pol.LineRev, 
			 sq.ReceivedDate, 
			 w.WarehouseID, 
			 w.Warehousename, 
			 w.ExternalID, 
			 sq.InTransit,
			 sq.isInspection
	UNION
	
	SELECT	'Purchase Order' 'Type', 
			pol.POLineID 'ID',
			pol.ItemID,
			po.AccountID,
			pol.StatusID 'StatusID', 
			s.StatusName,
			pol.Qty - ISNULL(rec.Qty,0) 'Qty', --Quantity on order
			pol.Qty - ISNULL(rec.Qty,0) - ISNULL(alloc.Allocated,0) 'Available', 
			pol.Cost,
			pol.DateCode,
			pol.PackagingID,
			pol.PackageConditionID,
			pol.PurchaseOrderID,
			pol.POVersionID,
			po.ExternalID 'POExternalID',
			pol.LineNum,
			pol.LineRev,
			dbo.fnGetObjectOwners(pol.PurchaseOrderID, 22) 'Buyers',
			pol.DueDate 'ShipDate',
			w.WarehouseID,
			w.WarehouseName,
			w.ExternalID 'WarehouseExternalID',
			NULL 'InTransit',
			NULL 'IsInspection'
	FROM vwPurchaseOrderLines pol	  
	  --Find all stock that was received against the PO line
	  LEFT OUTER JOIN (	SELECT sq.POLineID, SUM(sq.Qty) 'Qty'
					FROM vwStockQty sq
					WHERE IsDeleted = 0
					GROUP BY sq.POLineID
				) rec ON pol.POLineID = rec.POLineID
	  --Find all allocations to the PO line
	  LEFT OUTER JOIN (	SELECT po.POLineID, SUM(po.Qty - ISNULL(inv.Qty,0)) 'Allocated'
						FROM mapSOPOAllocation po
						--Find all allocations to the stock that was received against the PO line
						LEFT OUTER JOIN (	SELECT SOLineID, POLineID, SUM(Qty) 'Qty'
											FROM mapSOInvFulfillment m
											INNER JOIN ItemStock s ON m.StockID = s.StockID
											WHERE m.IsDeleted = 0
											GROUP BY SOlineID, POLineID
										) inv ON po.SOlineID = inv.SOlineID AND po.POLineID = inv.POLineID
						WHERE po.IsDeleted = 0 AND po.Qty - ISNULL(inv.Qty,0) > 0
						GROUP BY po.POLineID
					   ) alloc ON pol.POLineID = alloc.POLineID
	  INNER JOIN PurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID AND pol.POVersionID = po.VersionID
	  INNER JOIN lkpStatuses s ON pol.StatusID = s.StatusID AND s.ObjectTypeID = 23
	  LEFT OUTER JOIN Warehouses w ON po.ToWarehouseID = w.WarehouseID
	  WHERE pol.Qty - ISNULL(rec.Qty,0) > 0

GO


