/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.07.16
   Description:	Gets ItemStock sum from inspectionID
   Usage:		EXEC uspQCBreakdownReportSummaryGet @InspectionID = 10
   Revision History:
	
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCBreakdownReportSummaryGet]
(
	@InspectionID INT = NULL
)
AS
BEGIN
	DECLARE @OriginalIdentifiedStockID VARCHAR(50) = (SELECT TOP 1 ITS.ExternalID FROM ItemStockBreakdown B
														INNER JOIN ItemStock ITS on ITS.StockID = B.StockID
														INNER JOIN mapQCInspectionStock BR on BR.StockID = B.StockID
														WHERE BR.InspectionID = @InspectionID
														AND ISNULL(ITS.ExternalID ,'') != ''
														ORDER BY BR.Created ASC)
	SELECT TOP 1
		U.FirstName + ' ' + U.LastName CompletedByUser,
		CC.Comment Conclusion,
		ECCN,
		HTS,
		@InspectionID InspectionID,
		CONVERT(VARCHAR(50), @InspectionID) + ' ' + IT.TypeName InspectionIDTypeFormatted,
		ID.PartNumber,
		ID.PartNumber + ' {' + M.MfrName + '}' PartNumberManufacturer,
		SA.AccountName	CustomerAccount,
		PA.AccountName VendorAccount,
		CT.[Name] VendorCompanyType,
		SL.ProductSpec,
		C.CommodityName,
		SQ.StockID OriginalStockID,
		SQ.ExternalID LotNumber,
		W.WarehouseName,
		W.ExternalID + ' ' + W.warehousename WarehouseSite,
		IST.StatusName,
		I.CompletedDate,
		PO.PurchaseOrderID,
		dbo.fnReturnOrderDisplayID(PO.PurchaseOrderID, PO.ExternalID) PurchaseOrderDisplayID,
		PO.PONotes,
		SO.SalesOrderID,
		dbo.fnReturnOrderDisplayID(SO.SalesOrderID, SO.ExternalID) SalesOrderDisplayID,
		SO.QCNotes SOQCNotes,
		SL.CustomerPartNum,
		CONVERT(VARCHAR, SL.DueDate, 1) DueDate,
		CONVERT(VARCHAR, SL.ShipDate, 1) ShipDate,
		SQ.Qty ItemQty,
		ITS.ExternalID IdentifiedStockID,
		ITS.MfrLotNum,
		@OriginalIdentifiedStockID OriginalIdentifiedStockID
	FROM ItemStockBreakdown B
	LEFT OUTER JOIN ItemStock ITS on ITS.StockID = B.StockID
	LEFT OUTER JOIN mapQCInspectionStock BR on BR.StockID = B.StockID
	LEFT OUTER JOIN QCInspections I on I.InspectionID = BR.InspectionID
	INNER JOIN lkpQCInspectionTypes IT on IT.InspectionTypeID = I.InspectionTypeID
	LEFT OUTER JOIN Comments CC on CC.ObjectID = @InspectionID AND CC.CommentTypeID = 3
	LEFT OUTER JOIN Users U on U.UserID = I.CompletedBy
	LEFT OUTER JOIN vwStockQty SQ on SQ.StockID = B.StockID
	LEFT OUTER JOIN Items ID on ID.ItemID = SQ.ItemID
	INNER JOIN Manufacturers M on M.MfrID = ID.MfrID
	LEFT OUTER JOIN mapSOInvFulfillment SPA on SPA.StockID IN	--There should only be one Sales Order for a inspectiom stockID, but we dont want to match on @StockID or else we will only get it for one record
				  ( SELECT SQ.StockID
					FROM mapQCInspectionStock SQ
					WHERE InspectionID = @InspectionID )
	--Sales Order
	LEFT OUTER JOIN vwSalesOrderLines SL on SL.SOLineID = SPA.SOLineID
	LEFT OUTER JOIN vwSalesOrders SO on SO.SalesOrderID = SL.SalesOrderID
	LEFT OUTER JOIN Accounts SA on SA.AccountID = SO.AccountID
	--Purchase Order
	INNER JOIN vwPurchaseOrderLines PL on PL.POLineID = SQ.POLineID
	INNER JOIN vwPurchaseOrders PO on PO.PurchaseOrderID = PL.PurchaseOrderID
	LEFT OUTER JOIN Accounts PA on pA.AccountID = PO.AccountID
	LEFT OUTER JOIN lkpCompanyTypes CT on CT.CompanyTypeID = PA.CompanyTypeID

	LEFT OUTER JOIN lkpItemCommodities C on C.CommodityID = ID.CommodityID
	LEFT OUTER JOIN Warehouses W on W.WarehouseID = SQ.WarehouseID
	INNER JOIN lkpQCInspectionStatuses IST on IST.InspectionStatusID = I.InspectionStatusID
	WHERE I.InspectionID = @InspectionID
	AND B.StockID = ISNULL(@StockID, B.StockID)
	AND B.isDeleted = 0

END