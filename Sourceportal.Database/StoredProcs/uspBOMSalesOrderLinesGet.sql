/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.31
   Description:	Retrieves Sales Order lines
   Usage:		EXEC uspBOMSalesOrderLinesGet @SearchID = 106, @RowLimit = 10000, @SortBy = 'SoldPrice', @DescSort = 1
				EXEC [uspBOMSalesOrderLinesGet] @SearchID = 106, @PartNumber = 'bav'
				SELECT * FROM salesOrderLines WHERE PartNumberStrip LIKE 'AD8%'
				SELECT * FROM Searches
				SELECT * FROM SearchDetails WHERE SearchID = 3
   Revision History:
	   2017.09.14	AR	Added Pagination
	   2017.09.15	AR	Added SearchResults join
	   2017.09.21	AR	Moved logic to uspBomProcessMatch (Select here only)
	   2017.10.25	AR	Added BOM Columns
   ============================================= */

CREATE PROCEDURE [dbo].[uspBOMSalesOrderLinesGet]
(
	@SearchID INT,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		ItemID,
		R.Manufacturer Mfg,
		R.SalesOrderID,
		RecordID,
		SODate,
		AccountName Customer,
		PartNumber PartNumber,
		Qty QtySold,
		Price SoldPrice,
		DateCode DateCode,
		Cost UnitCost,
		DueDate,
		ShippedQty,
		OrderStatus,
		GrossProfitTotal,
		R.OwnerName SalesPerson,
		R.PriceDelta,
		R.Potential,
		R.BOMPrice,
		R.BOMPartNumber,
		R.BOMIntPartNumber,
		R.BOMMfg,
		R.BOMQty,
		--SalesPerson
		COUNT(*) OVER() AS 'TotalRows'
	FROM SearchResults R
	WHERE SearchID = @SearchID
	AND R.ResultType = 'S'


	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN OrderNumber
			WHEN @SortBy = 'RecordID' THEN RecordID
			WHEN @SortBy = 'OrderNumber' THEN OrderNumber
			WHEN @SortBy = 'QtySold' THEN Qty
			WHEN @SortBy = 'Mfg' THEN ItemID
		END
	END ASC,	
	--Date Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'SODate' THEN SODate
			WHEN @SortBy = 'DueDate' THEN DueDate
		END
	END ASC,
	--Float Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'SoldPrice' THEN Price
			WHEN @SortBy = 'UnitCost' THEN Cost
			WHEN @SortBy = 'GP' THEN GrossProfitTotal
		END
	END ASC,
	--Char Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Customer' THEN AccountName
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'DateCode' THEN DateCode
			WHEN @SortBy = 'SalesPerson' THEN OwnerName
		END
	END ASC,
	--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'RecordID' THEN RecordID
			WHEN @SortBy = 'OrderNumber' THEN OrderNumber
			WHEN @SortBy = 'QtySold' THEN Qty
		END
	END DESC,	
	--Date Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'SODate' THEN SODate
			WHEN @SortBy = 'DueDate' THEN DueDate
		END
	END DESC,
	--Float Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'SoldPrice' THEN Price
			WHEN @SortBy = 'UnitCost' THEN Cost
			WHEN @SortBy = 'Mfg' THEN ItemID
		END
	END DESC,
	--Char Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Customer' THEN AccountName
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'DateCode' THEN DateCode
			WHEN @SortBy = 'SalesPerson' THEN OwnerName
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
	/* 

	SELECT
		SL.SOLineID,
		LL.ItemListID,
		LL.ItemListLineID,
		SL.StatusID,
		SL.ItemID,
		LL.CommodityID,
		SL.CustomerPartNum,
		SL.PartNumberStrip,
		LL.Manufacturer,
		SL.Qty,
		LL.TargetPrice,
		SL.DueDate,
		SL.Created,
		SL.CreatedBy
	FROM SalesOrderLines SL
	LEFT OUTER JOIN ItemListLines LL on SL.PartNumberStrip  LIKE LL.PartNumberStrip + '%'

	UNION SELECT
	null,
	null, --R.rfq_no
	null,
	null,
	null,
	null,
	D.item_no,
	D.strip_item_no,
	D.manu_no,
	D.ord_qty,
	D.unit_cost,	
	D.due_date,
	LH.create_dt,
	0
	FROM [SourcePortal_Dev].epds01.ord_hedr H
	INNER JOIN [SourcePortal_Dev].epds01.ord_detl D ON H.order_no = D.order_no
	LEFT OUTER JOIN epds01.dbo.inv_trx IV ON d.order_no = IV.order_no AND D.lin_no = IV.lin_no AND IV.wx_type = 'S'
	LEFT OUTER JOIN epds01.dbo.lot_hedr LH ON IV.lot_no = LH.lot_no
	LEFT OUTER JOIN epds01.dbo.lot_info LI ON LH.li_int_id = LI.li_int_id

	OUTER APPLY (SELECT SUM(TR.trx_qty) trx_qty_SUM FROM epds01.dbo.inv_trx TR
				INNER JOIN epds01.dbo.lot_hedr H ON TR.lot_no = H.lot_no
				INNER JOIN epds01.dbo.lot_info I ON H.li_int_id = I.li_int_id
				WHERE TR.order_no =  D.order_no
							AND D.lin_no = TR.lin_no) TRX -- AND TR.wx_type = 'S'
	WHERE D.strip_item_no LIKE @PartNumber + '%'
	
	
	UNION SELECT
	null,
	null, --R.rfq_no
	null,
	null,
	null,
	null,
	D.item_no,
	D.strip_item_no,
	D.manu_no,
	D.ord_qty,
	D.unit_cost,	
	D.due_date,
	LH.create_dt,
	0
	FROM [SourcePortal_Dev].epds02.ord_hedr H
	INNER JOIN [SourcePortal_Dev].epds02.ord_detl D ON H.order_no = d.order_no
	LEFT OUTER JOIN epds02.dbo.inv_trx IV ON d.order_no = IV.order_no AND D.lin_no = IV.lin_no AND IV.wx_type = 'S'
	LEFT OUTER JOIN epds02.dbo.lot_hedr LH ON IV.lot_no = LH.lot_no
	LEFT OUTER JOIN epds02.dbo.lot_info LI ON LH.li_int_id = LI.li_int_id

	OUTER APPLY (SELECT SUM(TR.trx_qty) trx_qty_SUM FROM epds01.dbo.inv_trx TR
				INNER JOIN epds02.dbo.lot_hedr H ON TR.lot_no = H.lot_no
				INNER JOIN epds02.dbo.lot_info I ON H.li_int_id = I.li_int_id
				WHERE TR.order_no =  D.order_no
							AND D.lin_no = TR.lin_no) TRX -- AND TR.wx_type = 'S'
	WHERE D.strip_item_no LIKE @PartNumber + '%'

	
	UNION SELECT
	null,
	null, --R.rfq_no
	null,
	null,
	null,
	null,
	D.item_no,
	D.strip_item_no,
	D.manu_no,
	D.ord_qty,
	D.unit_cost,	
	D.due_date,
	LH.create_dt,
	0
	FROM [SourcePortal_Dev].epds03.ord_hedr H
	INNER JOIN [SourcePortal_Dev].epds03.ord_detl D ON H.order_no = d.order_no
	LEFT OUTER JOIN epds03.dbo.inv_trx IV ON d.order_no = IV.order_no AND D.lin_no = IV.lin_no AND IV.wx_type = 'S'
	LEFT OUTER JOIN epds03.dbo.lot_hedr LH ON IV.lot_no = LH.lot_no
	LEFT OUTER JOIN epds03.dbo.lot_info LI ON LH.li_int_id = LI.li_int_id

	OUTER APPLY (SELECT SUM(TR.trx_qty) trx_qty_SUM FROM epds01.dbo.inv_trx TR
				INNER JOIN epds03.dbo.lot_hedr H ON TR.lot_no = H.lot_no
				INNER JOIN epds03.dbo.lot_info I ON H.li_int_id = I.li_int_id
				WHERE TR.order_no =  D.order_no
							AND D.lin_no = TR.lin_no) TRX -- AND TR.wx_type = 'S'
	WHERE D.strip_item_no LIKE @PartNumber + '%'
	*/
END