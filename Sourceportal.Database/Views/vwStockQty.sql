CREATE OR ALTER VIEW [dbo].[vwStockQty]
AS 
	SELECT	ist.StockID,
			ist.POLineID,
			ist.ItemID,
			ist.InvStatusID,
			ist.IsRejected,
			SUM(ii.Qty) 'Qty',
			ist.ReceivedDate,
			ist.DateCode,
			ist.PackagingID,
			ist.PackageConditionID,
			ist.COO,
			ist.Expiry,
			ist.StockDescription,
			ist.ExternalID,
			MIN(wb.WarehouseID) 'WarehouseID',  --Shouldn't ever be in more than one warehouse
			MAX(CAST(ii.IsInspection AS INT)) 'IsInspection',
			CASE WHEN COUNT(*) <> COUNT(wb.ExternalUUID) THEN 1 ELSE 0 END 'InTransit',
			ist.IsDeleted
	FROM ItemStock ist
	INNER JOIN ItemInventory ii ON ist.StockID = ii.StockID AND ii.IsDeleted = 0 AND ii.Qty <> 0
	INNER JOIN WarehouseBins wb ON ii.WarehouseBinID = wb.WarehouseBinID
	GROUP BY	ist.StockID,
				ist.POLineID,
				ist.ItemID,
				ist.InvStatusID,
				ist.IsRejected,
				ist.ReceivedDate,
				ist.DateCode,
				ist.PackagingID,
				ist.PackageConditionID,
				ist.COO,
				ist.Expiry,
				ist.StockDescription,
				ist.ExternalID,
				ist.IsDeleted