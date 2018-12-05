

CREATE VIEW [dbo].[vwItemInventoryWithFulfillment]
AS 
	SELECT 	II.InventoryID,
			II.Qty 'InventoryQty',
			SIF.Qty 'FulfilledQty',			
			I.ItemID,		
			II.POLineID,
			SL.SOLineID,	
			II.DateCode,
			II.PackagingID,
			W.WarehouseID,
			I.CommodityID,		
			I.PartNumber,
			I.PartNumberStrip,
			I.MfrID,
			SO.AccountID 'CustomerAccountID',
			PO.AccountID 'SupplierAccountID'		
	FROM ItemInventory II
	  INNER JOIN WarehouseBins WB on WB.WarehouseBinID = II.WarehouseBinID
	  INNER JOIN Warehouses W on W.WarehouseID = WB.WarehouseID
	  INNER JOIN vwPurchaseOrderLines PL on PL.POLineID =  II.POLineID
	  INNER JOIN vwPurchaseOrders PO on PO.PurchaseOrderID = PL.PurchaseOrderID
	  LEFT OUTER JOIN (mapSOInvFulfillment SIF 
		INNER JOIN vwSalesOrderLines SL on SIF.SOLineID = SL.SOLineID
		INNER JOIN vwSalesOrders SO on SL.SalesOrderID = SO.SalesOrderID) on II.InventoryID = SIF.InventoryID AND SIF.IsDeleted = 0  
	  INNER JOIN Items I ON I.ItemID = II.ItemID  
	WHERE II.IsDeleted = 0