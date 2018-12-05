/* =============================================
   Author:			Aaron Rodecker
   Create date:		2017.08.31
   Description:		Retrieves Quote lines
   Usage:			EXEC uspBOMInventoryLinesGet @SearchID = 50
					EXEC uspBOMInventoryLinesGet @SearchID = 45 
				
   Revision History:
		2017.09.15	AR	Added SearchResults join
		2017.10.25	AR	Added BOM Columns
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMInventoryLinesGet]
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
	SELECT R.RecordID POLineID,
			R.ItemID,
			R.Manufacturer Manufacturer,
			R.WarehouseCode,
			R.Qty InventoryQty,
			R.PartNumber,
			R.Cost,
			ReservedQty,
			AvailableQty,
			R.DateCode,
			R.PriceDelta,
			R.Potential,
			R.BOMPrice,
			R.BOMPartNumber,
			R.BOMIntPartNumber,
			R.BOMMfg,
			R.BOMQty,
			COUNT(*) OVER() AS 'TotalRows'
	FROM SearchResults R
	WHERE SearchID = @SearchID 
	AND R.ResultType = 'I'
	
	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'ItemID' THEN ItemID
			WHEN @SortBy = 'POLineID' THEN RecordID
			WHEN @SortBy = 'InventoryQty' THEN Qty
			WHEN @SortBy = 'Cost' THEN Cost
			WHEN @SortBy = 'ReservedQty' THEN ReservedQty
			WHEN @SortBy = 'AvailableQty' THEN AvailableQty
		END
	END ASC,	

	--Date Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END ASC,
	--Float Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Cost' THEN Cost
		END
	END ASC,
	--Char Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			WHEN @SortBy = 'PartNumber' THEN PartNumber
		END
	END ASC,
		--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'ItemID' THEN ItemID
			WHEN @SortBy = 'POLineID' THEN RecordID
			WHEN @SortBy = 'InventoryQty' THEN Qty
			WHEN @SortBy = 'Cost' THEN Cost
			WHEN @SortBy = 'ReservedQty' THEN ReservedQty
			WHEN @SortBy = 'AvailableQty' THEN AvailableQty
		END
	END DESC,

	--Date Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END DESC,
	--Float Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Cost' THEN Cost
		END
	END DESC,
	--Char Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			WHEN @SortBy = 'PartNumber' THEN PartNumber
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
	/*
	UNION
		SELECT	0
				, 0
				, 0
				, 0
				, 0
				, item_no
				--, SUM(bal_of_lot) AS bal_of_lot
				--, epds01.dbo.calc_lot_cost(lot_no) AS lot_cost
				--, COALESCE(TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lot_no), 0)
				--, (COALESCE(Quantity, 0)) * (COALESCE(TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lot_no), 0))
				, 0
				, date_code
				--, COALESCE(SUM(bal_of_lot), 0) - COALESCE(SUM(qty_commit), 0)
				--, whse_cd
				, ReservedQty
				, AvailableQty
				--, Customer
				, tbl.PurchaseOrder
		FROM		 (SELECT H.item_no
							, H.manu_no
							, H.bal_of_lot
							--, epds01.lot_hedr.date_code
							, I.manu_date_code date_code
							, H.qty_commit
							, H.whse_cd
							, H.lot_no
							, H.orig_qty Quantity
							, OH.std_lbr_cst
							, H.qty_commit ReservedQty
							, H.bal_of_lot - H.qty_commit AvailableQty
							, OH.cust1 Customer
							, r.order_no PurchaseOrder
                 FROM       epds01.dbo.lot_hedr H
							JOIN epds01.dbo.lot_info I ON H.li_int_id = I.li_int_id
							LEFT OUTER JOIN (SELECT DISTINCT I.lot_no, I.order_no FROM epds01.dbo.inv_trx I WHERE I.trx_type = 'R') r ON H.lot_no = r.lot_no
							LEFT OUTER JOIN epds01.dbo.ord_hedr OH on OH.order_no = r.order_no
							WHERE H.item_no LIKE @PartNumber + '%' ) tbl
	GROUP BY	item_no, manu_no, date_code, whse_cd, epds01.dbo.calc_lot_cost(lot_no), Quantity, ReservedQty, AvailableQty, PurchaseOrder

	UNION
		SELECT	0
				, 0
				, 0
				, 0
				, 0
				, item_no
				--, SUM(bal_of_lot) AS bal_of_lot
				--, epds01.dbo.calc_lot_cost(lot_no) AS lot_cost
				--, COALESCE(TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lot_no), 0)
				--, (COALESCE(Quantity, 0)) * (COALESCE(TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lot_no), 0))
				, 0
				, date_code
				--, COALESCE(SUM(bal_of_lot), 0) - COALESCE(SUM(qty_commit), 0)
				--, whse_cd
				, ReservedQty
				, AvailableQty
				--, Customer
				, tbl.PurchaseOrder
		FROM		 (SELECT H.item_no
							, H.manu_no
							, H.bal_of_lot
							--, epds01.lot_hedr.date_code
							, I.manu_date_code date_code
							, H.qty_commit
							, H.whse_cd
							, H.lot_no
							, H.orig_qty Quantity
							, OH.std_lbr_cst
							, H.qty_commit ReservedQty
							, H.bal_of_lot - H.qty_commit AvailableQty
							, OH.cust1 Customer
							, r.order_no PurchaseOrder
                 FROM       epds02.dbo.lot_hedr H
							JOIN epds02.dbo.lot_info I ON H.li_int_id = I.li_int_id
							LEFT OUTER JOIN (SELECT DISTINCT I.lot_no, I.order_no FROM epds02.dbo.inv_trx I WHERE I.trx_type = 'R') r ON H.lot_no = r.lot_no
							LEFT OUTER JOIN epds02.dbo.ord_hedr OH on OH.order_no = r.order_no
							WHERE H.item_no LIKE @PartNumber + '%' ) tbl
	GROUP BY	item_no, manu_no, date_code, whse_cd, epds01.dbo.calc_lot_cost(lot_no), Quantity, ReservedQty, AvailableQty, PurchaseOrder

		UNION
		SELECT	0
				, 0
				, 0
				, 0
				, 0
				, item_no
				--, SUM(bal_of_lot) AS bal_of_lot
				--, epds01.dbo.calc_lot_cost(lot_no) AS lot_cost
				--, COALESCE(TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lot_no), 0)
				--, (COALESCE(Quantity, 0)) * (COALESCE(TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lot_no), 0))
				, 0
				, date_code
				--, COALESCE(SUM(bal_of_lot), 0) - COALESCE(SUM(qty_commit), 0)
				--, whse_cd
				, ReservedQty
				, AvailableQty
				--, Customer
				, tbl.PurchaseOrder
		FROM		 (SELECT H.item_no
							, H.manu_no
							, H.bal_of_lot
							--, epds01.lot_hedr.date_code
							, I.manu_date_code date_code
							, H.qty_commit
							, H.whse_cd
							, H.lot_no
							, H.orig_qty Quantity
							, OH.std_lbr_cst
							, H.qty_commit ReservedQty
							, H.bal_of_lot - H.qty_commit AvailableQty
							, OH.cust1 Customer
							, r.order_no PurchaseOrder
                 FROM       epds03.dbo.lot_hedr H
							JOIN epds03.dbo.lot_info I ON H.li_int_id = I.li_int_id
							LEFT OUTER JOIN (SELECT DISTINCT I.lot_no, I.order_no FROM epds01.dbo.inv_trx I WHERE I.trx_type = 'R') r ON H.lot_no = r.lot_no
							LEFT OUTER JOIN epds03.dbo.ord_hedr OH on OH.order_no = r.order_no
							WHERE H.item_no LIKE @PartNumber + '%' ) tbl
	GROUP BY	item_no, manu_no, date_code, whse_cd, epds01.dbo.calc_lot_cost(lot_no), Quantity, ReservedQty, AvailableQty, PurchaseOrder

*/
END