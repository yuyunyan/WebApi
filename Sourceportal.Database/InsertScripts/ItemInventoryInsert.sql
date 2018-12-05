INSERT INTO ItemInventory (POLineID, ItemID, InvStatusID, WarehouseID, Qty, DateCode, PackagingID, CreatedBy)
SELECT  POLineID, ItemID, 1, 1, Qty, DateCode, PackagingID, 0
FROM vwPurchaseOrderLines 
WHERE PurchaseOrderID % 3 = 1