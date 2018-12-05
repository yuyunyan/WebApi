/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.07
   Description:	Gets a list of Sales Order Lines that are not fulfilled completely
   Usage: EXEC uspSOLinesFulfillmentGet @ItemID=83, @Fulfilled = 0, @SearchString='', @RowOffset =0, @RowLimit=20, @SortBy='SalesOrderID', @CommentTypeID=10, @DescSort=1
		  EXEC uspSOLinesFulfillmentGet @Fulfilled = 2, @SearchString='', @RowOffset =0, @RowLimit=20, @SortBy='LineNum', @CommentTypeID=10, @DescSort=0
		  EXEC uspSOLinesFulfillmentGet @Fulfilled = 0 , @PoLineID=12
   Revision History:
			2017.08.22 BZ Add Comments
			2018.01.23 RV Added param PoLineId to filter matching sales order lines and the sum of those quantities
			2018.03.29 NA ***********THIS PROCEDURE HAS BEEN DEPRECATED.  USE uspOrderFulfillmentGet INSTEAD***********
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspSOLinesFulfillmentGet]
	@SearchString NVARCHAR(32) = '',
	@Fulfilled INT = 0, -- NOTE: 0 = Unfulfilled , 1 = Fulfilled , 2 = All (Both Fulfilled and Unfulfilled )
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = 0,
	@ItemID INT = NULL,
	@POLineID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

SELECT sol.SOLineID,
                     sol.SalesOrderID, 
                     sol.LineNum, 
                     a.AccountID,
                     a.AccountName,
                     sol.ItemID,
                     i.PartNumber,
                     m.MfrID,
                     m.MfrName,
                     ic.CommodityID,
                     ic.CommodityName,          
                     sol.Qty 'OrderQty',               
                     sol.Qty - (ISNULL(sopo.Qty, 0) - ISNULL(consumed.Qty, 0)) - ISNULL(soinv.Qty, 0) 'Remaining',
                     sopo.Qty 'AllocatedQty',
                     sol.Price,
                     sol.Cost,
                     sol.PackagingID,
                     p.PackagingName,
                     sol.DateCode,
                     sol.ShipDate,
                     sol.DueDate,
					 sopo.POLineID,
                     dbo.fnGetObjectOwners(sol.SalesOrderID, 16) 'Sellers',
                     COUNT(*) OVER() AS 'TotalRows',
                     dbo.fnGetCommentsCount(sol.SOLineID, @CommentTypeID) 'Comments'
       FROM vwSalesOrderLines sol
         INNER JOIN Items i ON sol.ItemID = i.ItemID 
         INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
         INNER JOIN lkpItemCommodities ic ON i.CommodityID = ic.CommodityID
         INNER JOIN vwSalesOrders so ON sol.SalesOrderID = so.SalesOrderID
         INNER JOIN Accounts a ON so.AccountID = a.AccountID
         INNER JOIN lkpStatuses st ON sol.StatusID = st.StatusID
         LEFT OUTER JOIN codes.lkpPackaging p ON sol.PackagingID = p.PackagingID
         LEFT OUTER JOIN (SELECT SOLineID, SUM(Qty) 'Qty' FROM mapSOInvFulfillment WHERE IsDeleted = 0 GROUP BY SOLineID) soinv ON sol.SOLineID = soinv.SOLineID
         LEFT OUTER JOIN (SELECT SOLineID, POLineID, SUM(Qty) 'Qty' FROM mapSOPOAllocation WHERE IsDeleted = 0  AND ISNULL(@POLineID, POLineID) = POLineID GROUP BY SOLineID , POLineID) sopo ON sol.SOLineID = sopo.SOLineID 
         LEFT OUTER JOIN (  SELECT sopo.SOLineID, ful.Qty 'Qty'
                                         FROM mapSOPOAllocation sopo
                                           INNER JOIN vwStockQty sq ON sopo.POLineID = sq.POLineID AND sq.IsDeleted = 0
                                           INNER JOIN mapSOInvFulfillment ful ON sq.StockID = ful.StockID AND sopo.SOLineID = ful.SOLineID AND ful.IsDeleted = 0
                                         WHERE sopo.IsDeleted = 0) consumed ON sol.SOLineID = consumed.SOLineID
       WHERE  
                (( @Fulfilled = 1 AND (sol.Qty - (ISNULL(sopo.Qty, 0) - ISNULL(consumed.Qty, 0)) - ISNULL(soinv.Qty, 0) = 0)) OR  
                ( @Fulfilled = 0 AND (sol.Qty - (ISNULL(sopo.Qty, 0) - ISNULL(consumed.Qty, 0)) - ISNULL(soinv.Qty, 0) > 0)) OR 
                ( @Fulfilled = 2 ))  
                AND (CAST(sol.SalesOrderID AS NVARCHAR(16)) + ISNULL(a.AccountName, '') + ISNULL(i.PartNumber, '') + ISNULL(p.PackagingName, '') + ISNULL(dbo.fnGetObjectOwners(sol.SalesOrderID, 16), '') LIKE '%' + ISNULL(@SearchString,'') + '%') 
                AND ( st.IsComplete = 0 AND st.IsCanceled = 0) 
                AND ISNULL(@ItemID, sol.ItemID) = sol.ItemID
				
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN sol.SalesOrderID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Mfr' THEN m.MfrName
				WHEN @SortBy = 'PartNumber' THEN i.PartNumber
				WHEN @SortBy = 'Customer' THEN a.AccountName
				WHEN @SortBy = 'Packaging' THEN p.PackagingName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'LineNum' THEN sol.LineNum
				WHEN @SortBy = 'DateCode' THEN sol.DateCode
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'SalesPerson' THEN dbo.fnGetObjectOwners(sol.SalesOrderID, 16)
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'OrderQty' THEN sol.Qty
				WHEN @SortBy = 'OrderNumber' THEN sol.SalesOrderID
				WHEN @SortBy = 'ReserceQty' THEN sol.Qty - (ISNULL(sopo.Qty, 0) - ISNULL(consumed.Qty, 0)) - ISNULL(soinv.Qty, 0)
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'Price' THEN sol.Price
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ShipDate' THEN sol.ShipDate
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN sol.SalesOrderID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Mfr' THEN m.MfrName
				WHEN @SortBy = 'PartNumber' THEN i.PartNumber
				WHEN @SortBy = 'Customer' THEN a.AccountName
				WHEN @SortBy = 'Packaging' THEN p.PackagingName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'LineNum' THEN sol.LineNum
				WHEN @SortBy = 'DateCode' THEN sol.DateCode
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'SalesPerson' THEN dbo.fnGetObjectOwners(sol.SalesOrderID, 16)
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'OrderQty' THEN sol.Qty
				WHEN @SortBy = 'OrderNumber' THEN sol.SalesOrderID
				WHEN @SortBy = 'ReserceQty' THEN sol.Qty - (ISNULL(sopo.Qty, 0) - ISNULL(consumed.Qty, 0)) - ISNULL(soinv.Qty, 0)
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'Price' THEN sol.Price
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ShipDate' THEN sol.ShipDate
			END
		END DESC, sol.SalesOrderID, sol.LineNum
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END

