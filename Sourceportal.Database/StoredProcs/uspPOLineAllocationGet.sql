/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.03.26
   Description:	Gets a list of Sales Order lines that a PO line is or can be allocated to.
   Usage: EXEC uspPOLineAllocationGet @POLineID = 414, @SOLineID = 89
		  EXEC uspPOLineAllocationGet @POLineID = 414, @PartNumber = 'CM8'

   Revision History:
		2018.08.13	NA	Added Warehouse
   Return Codes:
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspPOLineAllocationGet]
	@PartNumber VARCHAR(32) = NULL,
	@POLineID INT = NULL,
	@SOLineID INT = NULL,
	@IncludeUnallocated BIT = 0
AS
BEGIN
	
	DECLARE @POItemID INT = NULL,
			@PartNumberStrip VARCHAR(32) = NULL
	SELECT @POItemID = ItemID FROM vwPurchaseOrderLines WHERE POLineID = @POLineID

	IF @PartNumber IS NULL
		SELECT @PartNumberStrip = i.PartNumberStrip FROM vwPurchaseOrderLines pol INNER JOIN Items i ON pol.ItemID = i.ItemID WHERE POLineID = @POLineID
	ELSE
		SELECT @PartNumberStrip = dbo.fnStripNonAlphaNumeric(@PartNumber) + '%'

	SELECT	sol.SOLineID,
			sol.SalesOrderID,
			sol.SOVersionID,
			so.AccountID,
			a.AccountName,
			sol.ItemID,
			i.PartNumber,
			m.MfrName,
			sol.LineNum, 
			sol.StatusID,
			stl.StatusName,
			sol.Qty,
			sola.OrderQty - (sola.POAllocated + sola.InvAllocated) 'Needed',
			sopo.Qty 'Allocated',
			sol.Price,
			sol.DateCode,
			sol.ShipDate,
			so.ExternalID,
			sola.WarehouseID,
			w.WarehouseName,
			dbo.fnGetObjectOwners(sol.SalesOrderID, 16) 'Sellers',
			CASE WHEN sol.SOLineID = @SOLineID THEN 1 ELSE 0 END AS 'Target'
	FROM vwSalesOrderLines sol
	  INNER JOIN lkpStatuses stl ON sol.StatusID = stl.StatusID	  
	  INNER JOIN vwSalesOrders so ON sol.SalesOrderID = so.SalesOrderID
	  INNER JOIN lkpStatuses sto ON so.StatusID = sto.StatusID
	  INNER JOIN Accounts a ON so.AccountID = a.AccountID
	  INNER JOIN Items i ON sol.ItemID = i.ItemID
	  INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	  LEFT OUTER JOIN mapSOPOAllocation sopo ON sol.SOLineID = sopo.SOLineID AND sopo.POLineID = @POLineID AND sopo.IsDeleted = 0
	  LEFT OUTER JOIN vwSalesOrderLineAllocations sola ON sol.SOLineID = sola.SOLineID
	  LEFT OUTER JOIN Warehouses w ON sola.WarehouseID = w.WarehouseID
	WHERE sol.PartNumberStrip LIKE @PartNumberStrip
		--Exclude lines that are completed or canceled unless they have an existing allocation, or are the target SOLine
	  AND ((stl.IsComplete = 0 AND stl.IsCanceled = 0 AND sto.IsCanceled = 0 AND sto.IsComplete = 0) OR sol.SOLineID = @SOLineID OR sopo.Qty IS NOT NULL)
		--Toggle to include SO lines that don't have an existing allocation
	  AND (sopo.Qty IS NOT NULL OR @IncludeUnallocated = 1 OR sol.SOLineID = @SOLineID)
	ORDER BY [Target] DESC
END