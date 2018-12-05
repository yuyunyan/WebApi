-- =============================================
-- Author:				Nathan Ayers
-- Create date:			2018.02.06
-- Description:			Migrates data from EPDS databases into the lkpAccountHistory table for Vendor Rating
-- =============================================


--EXEC [replicated_epds02].[dbo].[vnd_perf_rpt] 

--@Vendors = '', 
--@VendOpt = 'ALL', 
--@FromDate = @StartDate, 
--@ToDate = @EndDate, 
--@BreakByPP = 'N', 
--@PPControl = '', 
--@PPID = 0, 
--@InclShop = 'N'

DELETE FROM SourcePortal2_DEV.dbo.lkpAccountHistory
--EPDS01
DECLARE @start_pos tinyint

CREATE TABLE #TMP_RESL (vend_no char(6), po_name char(40), order_no int, lot_no varchar(10), item_no char(25), manu_no char(6), whse_cd char(2), 
				lin_no int NULL, trx_date datetime, trx_qty int, orig datetime, promise_dt datetime, due_date datetime, in_route datetime NULL, trck_route char(1) NULL,
				po_dt datetime, pp_int_id int NULL, ship_asap bit)

CREATE TABLE #TMP_VENDOR (vend_no char(6) NOT NULL)

INSERT   #TMP_VENDOR
SELECT   vend_no
FROM   epds01.dbo.vendor
WHERE    vend_type = 'SUP'


Insert	#TMP_RESL
Select	vendor.vend_no, vendor.po_name, inv_trx.order_no, inv_trx.lot_no, inv_trx.item_no, inv_trx.manu_no, inv_trx.whse_cd,  
	inv_trx.lin_no, inv_trx.trx_date, sum(inv_trx.trx_qty),
		Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end orig, 
		Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end, 
		Case when po_detl.due_date = '1900-01-01' then po_detl.req_date else po_detl.due_date end, po_detl.in_route, vendor.trck_route,
	po_hedr.po_dt, NULL, po_detl.ship_asap
From		epds01.dbo.inv_trx
	JOIN     #TMP_VENDOR on #TMP_VENDOR.vend_no = inv_trx.vend_no
	JOIN	epds01.dbo.Vendor on Vendor.vend_no = #TMP_VENDOR.vend_no
	JOIN	epds01.dbo.po_detl on po_detl.po_no = inv_trx.order_no and po_detl.lin_no = inv_trx.lin_no
	JOIN	epds01.dbo.po_hedr on po_hedr.po_no = po_detl.po_no
Where	inv_trx.trx_type = 'R' 
	AND	inv_trx.trx_qty > 0
	AND	(inv_trx.adj_type is null or inv_trx.adj_type = '')
	and po_hedr.shop_po = 'N'
group by vendor.vend_no, vendor.po_name, inv_trx.order_no, inv_trx.lot_no, inv_trx.item_no, inv_trx.manu_no, inv_trx.whse_cd,
	inv_trx.lin_no, inv_trx.trx_date, 
	Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end , 
	Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end, 
	Case when po_detl.due_date = '1900-01-01' then po_detl.req_date else po_detl.due_date end, po_detl.in_route, vendor.trck_route,
	po_hedr.po_dt, po_detl.ship_asap
ORDER BY vendor.vend_no, inv_trx.order_no, inv_trx.lot_no, inv_trx.lin_no


UPDATE #TMP_RESL
SET PROMISE_DT = Case when NEW_PD = '1900-01-01' then promise_dt else new_pd end,
	 DUE_DATE = Case when NEW_DD = '1900-01-01' then due_date else NEW_DD end
FROM #TMP_RESL, epds01.dbo.PO_LOG
WHERE #TMP_RESL.ORDER_NO = PO_LOG.PO_NO and #TMP_RESL.LIN_NO = PO_LOG.LIN_NO AND TRX_TYPE = 'A'

INSERT INTO SourcePortal2_DEV.dbo.lkpAccountHistory (ExternalAccountID, ExternalRecordID, DataSource, AccountName, Quantity, DueDate, ReceivedDate, PartNumber, Manufacturer, OrderNumber, OrderLine, QuantityFailedQC)
SELECT 
		vend_no,
		lot_no,
		'epds01',
		po_name,
		trx_qty,
		due_date,
		trx_date,
		item_no,
		manu_no,
		order_no,
		lin_no,
		0
FROM #TMP_RESL
WHERE TRX_QTY <> 0
ORDER BY vend_no, order_no, lot_no, lin_no

DELETE FROM #TMP_RESL
DELETE FROM #TMP_VENDOR
SELECT @start_pos = NULL

--EPDS02

INSERT   #TMP_VENDOR
SELECT   vend_no
FROM   epds02.dbo.vendor
WHERE    vend_type = 'SUP'


Insert	#TMP_RESL
Select	vendor.vend_no, vendor.po_name, inv_trx.order_no, inv_trx.lot_no, inv_trx.item_no, inv_trx.manu_no, inv_trx.whse_cd,  
	inv_trx.lin_no, inv_trx.trx_date, sum(inv_trx.trx_qty),
		Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end orig, 
		Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end, 
		Case when po_detl.due_date = '1900-01-01' then po_detl.req_date else po_detl.due_date end, po_detl.in_route, vendor.trck_route,
	po_hedr.po_dt, NULL, po_detl.ship_asap
From		epds02.dbo.inv_trx
	JOIN     #TMP_VENDOR on #TMP_VENDOR.vend_no = inv_trx.vend_no
	JOIN	epds02.dbo.Vendor on Vendor.vend_no = #TMP_VENDOR.vend_no
	JOIN	epds02.dbo.po_detl on po_detl.po_no = inv_trx.order_no and po_detl.lin_no = inv_trx.lin_no
	JOIN	epds02.dbo.po_hedr on po_hedr.po_no = po_detl.po_no
Where	inv_trx.trx_type = 'R' 
	AND	inv_trx.trx_qty > 0
	AND	(inv_trx.adj_type is null or inv_trx.adj_type = '')
	and po_hedr.shop_po = 'N'
group by vendor.vend_no, vendor.po_name, inv_trx.order_no, inv_trx.lot_no, inv_trx.item_no, inv_trx.manu_no, inv_trx.whse_cd,
	inv_trx.lin_no, inv_trx.trx_date, 
	Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end , 
	Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end, 
	Case when po_detl.due_date = '1900-01-01' then po_detl.req_date else po_detl.due_date end, po_detl.in_route, vendor.trck_route,
	po_hedr.po_dt, po_detl.ship_asap
ORDER BY vendor.vend_no, inv_trx.order_no, inv_trx.lot_no, inv_trx.lin_no


UPDATE #TMP_RESL
SET PROMISE_DT = Case when NEW_PD = '1900-01-01' then promise_dt else new_pd end,
	 DUE_DATE = Case when NEW_DD = '1900-01-01' then due_date else NEW_DD end
FROM #TMP_RESL, epds02.dbo.PO_LOG
WHERE #TMP_RESL.ORDER_NO = PO_LOG.PO_NO and #TMP_RESL.LIN_NO = PO_LOG.LIN_NO AND TRX_TYPE = 'A'

INSERT INTO SourcePortal2_DEV.dbo.lkpAccountHistory (ExternalAccountID, ExternalRecordID, DataSource, AccountName, Quantity, DueDate, ReceivedDate, PartNumber, Manufacturer, OrderNumber, OrderLine, QuantityFailedQC)
SELECT 
		vend_no,
		lot_no,
		'epds02',
		po_name,
		trx_qty,
		due_date,
		trx_date,
		item_no,
		manu_no,
		order_no,
		lin_no,
		0
FROM #TMP_RESL
WHERE TRX_QTY <> 0
ORDER BY vend_no, order_no, lot_no, lin_no

DELETE FROM #TMP_RESL
DELETE FROM #TMP_VENDOR
SELECT @start_pos = NULL


--EPDS03

INSERT   #TMP_VENDOR
SELECT   vend_no
FROM   epds03.dbo.vendor
WHERE    vend_type = 'SUP'


Insert	#TMP_RESL
Select	vendor.vend_no, vendor.po_name, inv_trx.order_no, inv_trx.lot_no, inv_trx.item_no, inv_trx.manu_no, inv_trx.whse_cd,  
	inv_trx.lin_no, inv_trx.trx_date, sum(inv_trx.trx_qty),
		Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end orig, 
		Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end, 
		Case when po_detl.due_date = '1900-01-01' then po_detl.req_date else po_detl.due_date end, po_detl.in_route, vendor.trck_route,
	po_hedr.po_dt, NULL, po_detl.ship_asap
From		epds03.dbo.inv_trx
	JOIN     #TMP_VENDOR on #TMP_VENDOR.vend_no = inv_trx.vend_no
	JOIN	epds03.dbo.Vendor on Vendor.vend_no = #TMP_VENDOR.vend_no
	JOIN	epds03.dbo.po_detl on po_detl.po_no = inv_trx.order_no and po_detl.lin_no = inv_trx.lin_no
	JOIN	epds03.dbo.po_hedr on po_hedr.po_no = po_detl.po_no
Where	inv_trx.trx_type = 'R' 
	AND	inv_trx.trx_qty > 0
	AND	(inv_trx.adj_type is null or inv_trx.adj_type = '')
	and po_hedr.shop_po = 'N'
group by vendor.vend_no, vendor.po_name, inv_trx.order_no, inv_trx.lot_no, inv_trx.item_no, inv_trx.manu_no, inv_trx.whse_cd,
	inv_trx.lin_no, inv_trx.trx_date, 
	Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end , 
	Case when po_detl.promise_dt = '1900-01-01' then po_detl.req_date else po_detl.promise_dt end, 
	Case when po_detl.due_date = '1900-01-01' then po_detl.req_date else po_detl.due_date end, po_detl.in_route, vendor.trck_route,
	po_hedr.po_dt, po_detl.ship_asap
ORDER BY vendor.vend_no, inv_trx.order_no, inv_trx.lot_no, inv_trx.lin_no


UPDATE #TMP_RESL
SET PROMISE_DT = Case when NEW_PD = '1900-01-01' then promise_dt else new_pd end,
	 DUE_DATE = Case when NEW_DD = '1900-01-01' then due_date else NEW_DD end
FROM #TMP_RESL, epds03.dbo.PO_LOG
WHERE #TMP_RESL.ORDER_NO = PO_LOG.PO_NO and #TMP_RESL.LIN_NO = PO_LOG.LIN_NO AND TRX_TYPE = 'A'

INSERT INTO SourcePortal2_DEV.dbo.lkpAccountHistory (ExternalAccountID, ExternalRecordID, DataSource, AccountName, Quantity, DueDate, ReceivedDate, PartNumber, Manufacturer, OrderNumber, OrderLine, QuantityFailedQC)
SELECT 
		vend_no,
		lot_no,
		'epds03',
		po_name,
		trx_qty,
		due_date,
		trx_date,
		item_no,
		manu_no,
		order_no,
		lin_no,
		0
FROM #TMP_RESL
WHERE TRX_QTY <> 0
ORDER BY vend_no, order_no, lot_no, lin_no

DROP TABLE #TMP_RESL
DROP TABLE #TMP_VENDOR


--Update the rejected quantities
--EPDS01
UPDATE h
SET h.QuantityFailedQC = rej.trx_qty
FROM SourcePortal2_DEV.dbo.lkpAccountHistory h
INNER JOIN 
		(SELECT lot_no, SUM(trx_qty) 'trx_qty'
		FROM [epds01].[dbo].[dmr_rejects]
		GROUP BY lot_no) rej
	ON h.ExternalRecordID = LEFT(rej.lot_no, 7)
	AND h.DataSource = 'epds01'
--EPDS02
UPDATE h
SET h.QuantityFailedQC = rej.trx_qty
FROM SourcePortal2_DEV.dbo.lkpAccountHistory h
INNER JOIN 
		(SELECT lot_no, SUM(trx_qty) 'trx_qty'
		FROM [epds02].[dbo].[dmr_rejects]
		GROUP BY lot_no) rej
	ON h.ExternalRecordID = LEFT(rej.lot_no, 7)
	AND h.DataSource = 'epds02'
--EPDS03
UPDATE h
SET h.QuantityFailedQC = rej.trx_qty
FROM SourcePortal2_DEV.dbo.lkpAccountHistory h
INNER JOIN 
		(SELECT lot_no, SUM(trx_qty) 'trx_qty'
		FROM [epds03].[dbo].[dmr_rejects]
		GROUP BY lot_no) rej
	ON h.ExternalRecordID = LEFT(rej.lot_no, 7)
	AND h.DataSource = 'epds03'

