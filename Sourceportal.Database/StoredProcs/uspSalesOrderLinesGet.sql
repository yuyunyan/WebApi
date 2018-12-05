/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Gets the line items that make up a Sales Order
   Usage: EXEC uspSalesOrderLinesGet @SOLineID = 7
		  EXEC uspSalesOrderLinesGet @SalesOrderID = 100007, @SOVersionID = 2

   Revision History:
			2017.07.28	NA	Added DueDate, removed GPM calculation, consolidated sort logic
			2017.08.22  BZ  Added Comments
			2017.12.27  ML  Add CommodityID to Select
			2018.02.06  CT  Add ProductSpec to Select
			2018.03.08	NA	Added Package Condition
			2018.03.14	BZ	Added Delivery Rule
			2018.04.06	NA	Converted to new allocation view, added MissingRTP flag
			2018.06.14	AR	Added partNumberManufacturer column & SAP placeholder columns, (SO report), BalanceShippedReportQty col, Proforma report
			2018.06.21	AR	Added CountryOfOrigin (COO) from mapSalesOrderLineShipments, Added HTS/ECCN/WeightG from items
			2018.06.25	NA	Converted to ItemStock schema
			2018.07.30	NA	Added ProcWarehouse
			2018.08.09	NA	Removed ProcWarehouse, Added DeliveryStatus and InvoiceStatus
   Return Codes:

   ============================================= */
CREATE PROCEDURE [dbo].[uspSalesOrderLinesGet]
	@SOLineID INT = NULL,
	@SalesOrderID INT = NULL,
	@SOVersionID INT = NULL,	
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = 0
AS
BEGIN
	SELECT
			sol.SOLineID
			, sol.SalesOrderID
			, sol.StatusID
			, s.StatusName
			, s.IsCanceled 'StatusIsCanceled'
			, sol.LineNum			
			, sol.ItemID
			, sol.CustomerLine
			, i.PartNumber
			, m.MfrName		
			, i.PartNumber + '<br>' + m.Mfrname PartNumberManufacturer	
			, i.PartDescription
			, c.CommodityName
			, c.CommodityID
			, sol.CustomerPartNum
			, sol.Qty
			, CONVERT(VARCHAR(15), sol.Price * sol.Qty, 1) + ' ' + A.CurrencyID NetPrice
			, CONVERT(VARCHAR(15), sol.Price, 1) + ' ' + A.CurrencyID AS PriceFormatted
			, (ISNULL(sola.POAllocated, 0) + ISNULL(sola.InvAllocated, 0)) AS Reserved
			, CONVERT(VARCHAR(15), sol.Price, 1) + ' ' + A.CurrencyID 
				+ ' <br>' + CONVERT(VARCHAR(15), sol.Cost, 1) + ' ' + A.CurrencyID AS UnitPriceUnitCostFormatted
			, CONVERT(VARCHAR(15), sol.Price * sol.Qty, 1) + ' ' + A.CurrencyID 
				+ ' <br>' + CONVERT(VARCHAR(15), sol.Cost * sol.Qty, 1) + ' ' + A.CurrencyID AS ExtPriceExtCost
			, sol.Price
			, sol.Cost			
			, sol.DateCode
			, sol.DeliveryRuleID
			, sol.PackagingID
			, p.PackagingName
			, sol.PackageConditionID
			, pc.ConditionName
			, NULLIF(sol.ShipDate,'') ShipDate
			, NULLIF(sol.DueDate, '') DueDate
			, sol.ProductSpec
			, sol.DeliveryStatus
			, sol.InvoiceStatus
			, w.WarehouseName 'ProcWarehouse'
			, sol.Qty - MS.ShipmentQty BalanceShippedQty
			, FORMAT(sol.DueDate, 'dd MMM yyy') + ' <br>' + FORMAT(sol.ShipDate, 'dd MMM yyy') DueDateShipDateFormatted
			, CASE WHEN ISNULL( MS.ShipmentQty,0) = 0 THEN sol.Qty WHEN sol.Qty = MS.ShipmentQty THEN 0 ELSE sol.Qty - MS.ShipmentQty END BalanceShippedReportQty
			, FORMAT(sol.ShipDate, 'dd MMM yyy') + '<br>' + FORMAT(sol.DueDate, 'dd MMM yyy') ShipDateDueDate
			, CASE WHEN ISNULL(r.RoutesCount, 0) = 0 THEN 1 ELSE 0 END 'MissingRTP'			
			, 0 'CommentCount' --Placeholder
			, COUNT(*) OVER() AS 'TotalRows'
			, dbo.fnGetCommentsCount(sol.SOLineID, @CommentTypeID) 'Comments'
			, F.COO
			, I.HTS
			, I.ECCN
			, I.WeightG
	FROM SalesOrderLines sol
	  LEFT OUTER JOIN lkpStatuses s ON sol.StatusID = s.StatusID AND s.IsDeleted = 0
	  INNER JOIN SalesOrders SO on SO.SalesOrderID = sol.SalesOrderID AND SO.VersionID = @SOVersionID
	  LEFT OUTER JOIN Accounts A on A.AccountID = SO.AccountID
	  INNER JOIN Items i ON sol.ItemID = i.ItemID
	  INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	  INNER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID
	  LEFT OUTER JOIN Warehouses w ON sol.ProcWarehouseID = w.WarehouseID
	  LEFT OUTER JOIN codes.lkpPackaging p ON sol.PackagingID = p.PackagingID
	  LEFT OUTER JOIN codes.lkpPackageConditions pc ON sol.PackageConditionID = pc.PackageConditionID
	  LEFT OUTER JOIN vwSalesOrderLineAllocations sola ON sol.SOLineID = sola.SOLineID
	  LEFT OUTER JOIN (SELECT SOLineID, COUNT(*) 'RoutesCount' FROM mapBuyerSORoutes WHERE IsDeleted = 0 GROUP BY SOLineID) r ON sol.SOLineID = r.SOLineID
	  OUTER APPLY (SELECT SUM(Qty) ShipmentQty FROM mapSalesOrderLineShipments MS WHERE MS.SOLineID = sol.SOLineID ) MS 
	  OUTER APPLY (SELECT TOP 1 II.COO FROM ItemStock II
					  INNER JOIN mapSOInvFulfillment F on F.StockID = II.StockID
					  WHERE SOLineID = sol.SOLineID
					  AND II.COO IS NOT NULL) F
	WHERE (sol.SOLineID = @SOLineID
			OR (sol.SalesOrderID = @SalesOrderID AND sol.SOVersionID = @SOVersionID))
	  AND sol.IsDeleted = 0
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN sol.LineNum
				WHEN @SortBy = 'Qty' THEN sol.Qty
				WHEN @SortBy = 'Price' THEN sol.Price
				WHEN @SortBy = 'Cost' THEN sol.Cost
				WHEN @SortBy = 'CustomerLine' THEN sol.CustomerLine
				WHEN @SortBy = 'GPM' THEN CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ShipDate' THEN sol.ShipDate
				WHEN @SortBy = 'DueDate' THEN sol.DueDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'PackageCondition' THEN pc.ConditionName
				WHEN @SortBy = 'CustomerPartNum' THEN sol.CustomerPartNum
				WHEN @SortBy = 'CommodityName' THEN c.CommodityName
				WHEN @SortBy = 'PartNumber' THEN i.PartNumber
				WHEN @SortBy = 'StatusName' THEN s.StatusName
				WHEN @SortBy = 'MfrName' THEN m.MfrName
				WHEN @SortBy = 'DeliveryStatus' THEN sol.DeliveryStatus
				WHEN @SortBy = 'InvoiceStatus' THEN sol.InvoiceStatus
				
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN sol.LineNum
				WHEN @SortBy = 'Qty' THEN sol.Qty
				WHEN @SortBy = 'Price' THEN sol.Price
				WHEN @SortBy = 'Cost' THEN sol.Cost
				WHEN @SortBy = 'CustomerLine' THEN sol.CustomerLine
				WHEN @SortBy = 'GPM' THEN CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ShipDate' THEN sol.ShipDate
				WHEN @SortBy = 'DueDate' THEN sol.DueDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'PackageCondition' THEN pc.ConditionName
				WHEN @SortBy = 'CustomerPartNum' THEN sol.CustomerPartNum
				WHEN @SortBy = 'CommodityName' THEN c.CommodityName
				WHEN @SortBy = 'PartNumber' THEN i.PartNumber
				WHEN @SortBy = 'StatusName' THEN s.StatusName
				WHEN @SortBy = 'MfrName' THEN m.MfrName
				WHEN @SortBy = 'DeliveryStatus' THEN sol.DeliveryStatus
				WHEN @SortBy = 'InvoiceStatus' THEN sol.InvoiceStatus
			END
		END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY	  
END