/* =============================================
   Author:			Aaron Rodecker
   Create date:		2017.08.31
   Description:		Retrieves Quote lines
   Usage:			EXEC uspBOMQuoteLinesGet @PartNumber = 'AD8'
   
   Revision History:
		2017.09.15	AR	Added SearchResults join
		2017.10.25	AR	Added BOM Columns
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMQuoteLinesGet]
(
	@SearchID INT,
	@PartNumber VARCHAR(128) = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT F.ItemID,
			R.OrderNumber,
			QL.Created SODate,
			R.AccountID Customer,
			R.PartNumber,
			R.Manufacturer,
			R.Qty BOMQty,
			QL.Qty QtySold,
			R.Price BOMPrice,
			QL.Price SoldPrice,
			R.PriceDelta,
			R.Potential,
			R.DateCode,
			--Unit Cost,
			--GP,
			R.DueDate,
			R.ShippedQty,
			R.OrderStatus,
			R.PriceDelta,
			R.Potential,
			R.BOMPrice,
			R.BOMPartNumber,
			R.BOMIntPartNumber,
			R.BOMMfg,
			R.BOMQty,
			COUNT(*) OVER() AS 'TotalRows'
			--SalesPerson
	FROM SearchResults R
	LEFT OUTER JOIN vwItemInventoryWithFulfillment F on F.POLineID = R.PurchaseOrderID AND R.DataSource NOT IN (1,2,3)
	LEFT OUTER JOIN QuoteLines QL on QL.PartNumberStrip  LIKE R.PartNumber + '%'
	WHERE R.PartNumber LIKE @PartNumber + '%'
	AND R.SearchID = ISNULL(@SearchID,R.SearchID)
	AND R.ResultType = 'Q'
	ORDER BY OrderNumber ASC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
	/*
	UNION SELECT
	0
	, D.lin_no
	, 0
	, 0
	, 0
	, D.item_no
	, D.strip_item_no
	, D.manu_no
	, D.orig_qty
	, D.target_prc
	, D.create_dt
	, 0
	, 0
	FROM epds01.dbo.quote Q
	INNER JOIN epds01.dbo.quo_detl D ON Q.quote_no = D.quote_no
	WHERE D.strip_item_no LIKE @PartNumber + '%'

	UNION SELECT
	0
	, D.lin_no
	, 0
	, 0
	, 0
	, D.item_no
	, D.strip_item_no
	, D.manu_no
	, D.orig_qty
	, D.target_prc
	, D.create_dt
	, 0
	, 0
	FROM epds02.dbo.quote Q
	INNER JOIN epds02.dbo.quo_detl D ON Q.quote_no = D.quote_no
	WHERE D.strip_item_no LIKE @PartNumber + '%'

	UNION SELECT
	0
	, D.lin_no
	, 0
	, 0
	, 0
	, D.item_no
	, D.strip_item_no
	, D.manu_no
	, D.orig_qty
	, D.target_prc
	, D.create_dt
	, 0
	, 0
	FROM epds03.dbo.quote Q
	INNER JOIN epds03.dbo.quo_detl D ON Q.quote_no = D.quote_no
	WHERE D.strip_item_no LIKE @PartNumber + '%'
	*/
END