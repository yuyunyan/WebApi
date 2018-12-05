CREATE VIEW [dbo].[vwStockWithFulfillment]
AS
	SELECT	sq.StockID,
			sq.POLineID,
			sq.ItemID,
			i.PartNumber,
			i.PartNumberStrip,
			i.MfrID,
			i.CommodityID,
			sq.InvStatusID,
			sq.Qty 'StockQty',
			soinv.Qty 'FulfilledQty',
			sq.ReceivedDate,
			sq.DateCode,
			sq.PackagingID,
			sq.PackageConditionID,
			sq.COO,
			sq.Expiry,
			sq.StockDescription,
			sq.WarehouseID,
			w.WarehouseName,
			sq.ExternalID,
			so.AccountID 'CustomerAccountID',
			po.AccountID 'VendorAccountID'
	FROM vwStockQty sq
	INNER JOIN Warehouses w ON sq.WarehouseID = w.WarehouseID
	INNER JOIN vwPurchaseOrderLines pol ON sq.POLineID = pol.POLineID
	INNER JOIN vwPurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID
	LEFT OUTER JOIN (mapSOInvFulfillment soinv
		INNER JOIN vwSalesOrderLines sol ON soinv.SOLineID = sol.SOLineID
		INNER JOIN vwSalesOrders so ON sol.SalesOrderID = so.SalesOrderID) ON sq.StockID = soinv.StockID AND soinv.IsDeleted = 0
	INNER JOIN items i ON sq.ItemID = i.ItemID
	WHERE sq.IsRejected = 0 AND sq.IsDeleted = 0
	