/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.31
   Description:	Retrieves Purchase Order lines
   Usage:		EXEC uspBOMPurchaseOrderLinesGet @SearchID = 269
				SELECT * FROM Searches
				SELECT * FROM SearchResults WHERE SearchID = 219
   Revision History:
	   2017.09.15	AR	Added SearchResults join
	   2017.09.22	AR	Moved query logic to uspBomProcessMatch for caching
	   2017.10.25	AR	Added BOM Columns
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMPurchaseOrderLinesGet]
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
	SELECT Manufacturer Mfg,
			OrderNumber PurchaseOrderID,
			R.PartNumber MfgPartNumber,
			R.AccountName Vendor,
			OrderDate PODate,
			RecordID POLineID,
			DateCode,
			AccountName Customer,
			PartNumber,
			Qty QtyOrdered,
			Cost POCost,
			ReceivedDate,
			ReceivedQty,
			OrderStatus,
			Buyer,
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
	WHERE SearchID = @SearchID
	AND R.ResultType = 'P'
	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN OrderNumber
			WHEN @SortBy = 'PONumber' THEN PurchaseOrderID
			WHEN @SortBy = 'POLineID' THEN RecordID
			WHEN @SortBy = 'QtyOrdered' THEN Qty
			WHEN @SortBy = 'Mfg' THEN ItemID
		END
	END ASC,	

	--Date Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'ReceivedDate' THEN ReceivedDate
		END
	END ASC,
	--Float Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'POCost' THEN Cost
		END
	END ASC,
	--Char Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Customer' THEN AccountName
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'DateCode' THEN DateCode
			WHEN @SortBy = 'Buyer' THEN Buyer
			WHEN @SortBy = 'OrderStatus' THEN OrderStatus
		END
	END ASC,
		--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN OrderNumber
			WHEN @SortBy = 'PONumber' THEN PurchaseOrderID
			WHEN @SortBy = 'POLineID' THEN RecordID
			WHEN @SortBy = 'QtyOrdered' THEN Qty
			WHEN @SortBy = 'Mfg' THEN ItemID
		END
	END DESC,	
	--Date Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'ReceivedDate' THEN ReceivedDate
		END
	END DESC,
	--Float Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'POCost' THEN Cost
		END
	END DESC,
	--Char Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Customer' THEN AccountName
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'DateCode' THEN DateCode
			WHEN @SortBy = 'Buyer' THEN Buyer
			WHEN @SortBy = 'OrderStatus' THEN OrderStatus
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
	/*
	UNION SELECT
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
	H.create_dt,
	0
	FROM [SourcePortal_Dev].epds01.po_hedr H
	INNER JOIN [SourcePortal_Dev].epds01.po_detl D ON H.po_no = D.po_no
	INNER JOIN epds01.dbo.vendor V ON H.vend_no = V.vend_no
	
	OUTER APPLY (SELECT DISTINCT MAX(TR.trx_date) trx_date FROM epds01.dbo.po_detl D -- join inv_trx on ord_detl and ord_detl ON item_no TOP 1 RECENT (SUBQUERY)
					INNER JOIN epds01.dbo.inv_trx TR on TR.order_no = D.po_no
					WHERE TR.lin_no = D.lin_no AND TR.order_no = D.po_no AND TR.trx_type = 'R' AND TR.trx_qty <> 0) TRX				

	WHERE D.strip_item_no LIKE @PartNumber + '%'

	UNION SELECT
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
	H.create_dt,
	0
	FROM [SourcePortal_Dev].epds02.po_hedr H
	INNER JOIN [SourcePortal_Dev].epds02.po_detl D ON H.po_no = D.po_no
	INNER JOIN epds02.dbo.vendor V ON H.vend_no = V.vend_no
	
	OUTER APPLY (SELECT DISTINCT MAX(TR.trx_date) trx_date FROM epds02.dbo.po_detl D -- join inv_trx on ord_detl and ord_detl ON item_no TOP 1 RECENT (SUBQUERY)
					INNER JOIN epds02.dbo.inv_trx TR on TR.order_no = D.po_no
					WHERE TR.lin_no = D.lin_no AND TR.order_no = D.po_no AND TR.trx_type = 'R' AND TR.trx_qty <> 0) TRX				

	WHERE D.strip_item_no LIKE @PartNumber + '%'

	UNION SELECT
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
	H.create_dt,
	0
	FROM [SourcePortal_Dev].epds03.po_hedr H
	INNER JOIN [SourcePortal_Dev].epds03.po_detl D ON H.po_no = D.po_no
	INNER JOIN epds03.dbo.vendor V ON H.vend_no = V.vend_no
	
	OUTER APPLY (SELECT DISTINCT MAX(TR.trx_date) trx_date FROM epds03.dbo.po_detl D -- join inv_trx on ord_detl and ord_detl ON item_no TOP 1 RECENT (SUBQUERY)
					INNER JOIN epds03.dbo.inv_trx TR on TR.order_no = D.po_no
					WHERE TR.lin_no = D.lin_no AND TR.order_no = D.po_no AND TR.trx_type = 'R' AND TR.trx_qty <> 0) TRX				

	WHERE D.strip_item_no LIKE @PartNumber + '%'
	*/

END
