/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.08.03
   Description:	Gets the line items that make up a Sales Order
   Usage: EXEC uspPurchaseOrderLinesGet @PurchaseOrderID = 100009, @POVersionID = 2


    Revision History:
			2017.08.22 BZ Add Comments
			2018.01.19 Added logic to get total allocated qty for a purchase order line
			2018.03.08	NA	Added Package Condition
			2018.05.17	BZ	Added PoId and POVersionID
			2018.05.18	BZ	Added SoId and SoVersionID
			2018.05.22	NA	Added LineRev
			2018.06.04	AR	Added report columns and accounts tbl JOIN
			2018.07.13	NA	Added ToWarehouseID and WarehouseName
			2018.08.08	NA	Changed to return deleted line if specific @POLineID is provided
   Return Codes:

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspPurchaseOrderLinesGet]
	@POLineID INT = NULL,
	@PurchaseOrderID INT = NULL,
	@POVersionID INT = NULL,	
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = 0
AS
BEGIN
	SELECT 
			pol.POLineID
			, pol.StatusID
			, s.StatusName
			, s.IsCanceled 'StatusIsCanceled'
			, pol.LineNum
			, pol.LineRev
			, pol.VendorLine			
			, pol.ItemID
			, i.PartNumber
			, m.MfrName			
			, i.PartNumber + ' / ' + m.MfrName PartNumberManufacturer
			, c.CommodityName
			, pol.Qty
			, CONVERT(VARCHAR(15), pol.Qty * pol.cost, 1) + ' ' + A.CurrencyID NetPrice
			, CONVERT(VARCHAR(15), pol.Cost, 1) + ' ' + A.CurrencyID 'PriceFormatted'
			, pol.Cost
			, pol.DateCode
			, pol.PackagingID
			, p.PackagingName
			, pol.PurchaseOrderID
			, pol.POVersionID
			, pol.ClonedFromID
			, pol.PackageConditionID
			, pc.ConditionName
			, pol.ToWarehouseID
			, w.WarehouseName
			, pol.DueDate	
			, CONVERT(CHAR(15), pol.DueDate, 106)	DueDateFormatted		
			, pol.IsSpecBuy
			, pol.SpecBuyForAccountID
			, pol.SpecBuyForUserID
			, pol.SpecBuyReason
			, sopo.AllocatedQty
			, COUNT(*) OVER() AS 'TotalRows'
			, sol.SalesOrderID
			, sol.SOVersionID
			, so.ExternalID
			, dbo.fnGetCommentsCount(pol.POLineID, @CommentTypeID) 'Comments'
			, A.PONotes
	FROM PurchaseOrderLines pol  
	  LEFT OUTER JOIN Accounts A on A.AccountID = (SELECT AccountID FROM PurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID AND VersionID = @POVersionID)
	  LEFT OUTER JOIN lkpStatuses s ON pol.StatusID = s.StatusID AND s.IsDeleted = 0
	  INNER JOIN Items i ON pol.ItemID = i.ItemID
	  INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	  INNER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID AND c.IsDeleted = 0
	  LEFT OUTER JOIN codes.lkpPackaging p ON pol.PackagingID = p.PackagingID
	  LEFT OUTER JOIN codes.lkpPackageConditions pc ON pol.PackageConditionID = pc.PackageConditionID
	  LEFT OUTER JOIN (SELECT SOLineID, POLineID, SUM(Qty) 'AllocatedQty' FROM mapSOPOAllocation WHERE IsDeleted = 0 GROUP BY POLineID, SOLineID ) sopo ON pol.POLineID = sopo.POLineID 
	  LEFT OUTER JOIN vwSalesOrderLines sol ON sol.SOLineID = sopo.SOLineID
	  LEFT OUTER JOIN Warehouses w ON pol.ToWarehouseID = w.WarehouseID
	  LEFT OUTER JOIN SalesOrders SO on SO.SalesOrderID = sol.SalesOrderID AND SOVersionID = sol.SOVersionID
	WHERE (pol.POLineID = @POLineID
			OR (pol.PurchaseOrderID = @PurchaseOrderID AND pol.POVersionID = @POVersionID))
	  AND (pol.IsDeleted = 0 OR @POLineID IS NOT NULL)  
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN pol.LineNum
				WHEN @SortBy = 'Qty' THEN pol.Qty
				WHEN @SortBy = 'Cost' THEN pol.Cost
				/*WHEN @SortBy = 'GPM' THEN CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END*/
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'DueDate' THEN pol.DueDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'ConditionName' THEN pc.ConditionName
				WHEN @SortBy = 'CommodityName' THEN c.CommodityName
				WHEN @SortBy = 'PartNumber' THEN i.PartNumber
				WHEN @SortBy = 'StatusName' THEN s.StatusName
				WHEN @SortBy = 'MfrName' THEN m.MfrName
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN pol.LineNum
				WHEN @SortBy = 'Qty' THEN pol.Qty
				WHEN @SortBy = 'Cost' THEN pol.Cost
				/*WHEN @SortBy = 'CustomerLine' THEN sol.CustomerLine
				WHEN @SortBy = 'GPM' THEN CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END*/
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'DueDate' THEN pol.DueDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'ConditionName' THEN pc.ConditionName
				/*WHEN @SortBy = 'CustomerPartNum' THEN pol.CustomerPartNum*/
				WHEN @SortBy = 'CommodityName' THEN c.CommodityName
				WHEN @SortBy = 'PartNumber' THEN i.PartNumber
				WHEN @SortBy = 'StatusName' THEN s.StatusName
				WHEN @SortBy = 'MfrName' THEN m.MfrName
			END
		END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
	  
END