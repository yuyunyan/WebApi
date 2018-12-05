/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.08.13
   Description:	Returns the PO and Inventory allocation counts for a given Sales Order Line
   Usage:	EXEC uspSalesOrderLineAllocationsGet @SOLineID = 1233

   Return Codes:

   Revision History:

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderLineAllocationsGet]
	@SOLineID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Temp TABLE (PurchaseOrderID INT, POExternalID VARCHAR(50), LineNum INT, LineRev INT, POLineID INT, PreAllocated INT, Received INT, FromStock INT)
	
	--Purchase Order Allocations
	INSERT INTO @Temp
	SELECT	po.PurchaseOrderID,
			po.ExternalID, 
			pol.LineNum,
			pol.LineRev, 
			pol.POLineID, 
			sopo.Qty, 
			sq.Qty, 
			NULL
	FROM mapSOPOAllocation sopo
	INNER JOIN PurchaseOrderLines pol ON sopo.POLineID = pol.POLineID
	INNER JOIN PurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID AND pol.POVersionID = po.VersionID
	LEFT OUTER JOIN (SELECT SUM(Qty) 'Qty', POLineID FROM vwStockQty WHERE IsDeleted = 0 GROUP BY POLineID) sq ON pol.POLineID = sq.POLineID
	WHERE SOLineID = @SOLineID
	AND sopo.IsDeleted = 0
	
	--Merge the inventory allocations that match PO allocations, then insert those that are just allocated to the inventory
	MERGE @Temp AS t
	USING (	SELECT 
				po.PurchaseOrderID,
				po.ExternalID, 
				pol.LineNum,
				pol.LineRev, 
				pol.POLineID,
				SUM(sq.Qty) 'Received', 
				SUM(ful.Qty) 'FromStock'
			FROM mapSOInvFulfillment ful
			INNER JOIN vwStockQty sq ON ful.StockID = sq.StockID AND sq.IsDeleted = 0
			INNER JOIN PurchaseOrderLines pol ON sq.POLineID = pol.POLineID
			INNER JOIN PurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID AND pol.POVersionID = po.VersionID
			WHERE SOLineID = @SOLineID
			AND ful.IsDeleted = 0
			GROUP BY po.PurchaseOrderID, po.ExternalID, pol.LineNum, pol.LineRev, pol.POLineID) r ON t.POLineID = r.POLineID
	WHEN NOT MATCHED THEN
		INSERT (PurchaseOrderID, POExternalID, LineNum, LineRev, POLineID, Received, FromStock)
		VALUES (r.PurchaseOrderID, r.ExternalID, r.LineNum, r.LineRev, r.POLineID, r.Received, r.FromStock)
	WHEN MATCHED
		THEN UPDATE SET t.FromStock = r.FromStock;

	SELECT * FROM @Temp
END