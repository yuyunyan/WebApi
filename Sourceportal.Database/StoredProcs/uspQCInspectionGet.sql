/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.15
   Description:	Retrieves the details for an inspection
   Usage:		EXEC uspQCInspectionGet 100, 1

   Revision History: 
   2017.0.25	AR	Changed inner joins to outer joins 
   2017.08.25	ML	Added Received date and Ship date
   2018.02.07	Rv Added Sales order QC Notes
   2018.04.24   ML Added ExternalId and InspectionStatusID
   2018.05.02   ML Added ResultId
   2018.05.03   ML Added DecisionCode and AcceptanceCode
   2018.05.09   CT Added InspectionQty
   2018.05.14   JT Added UserId
   2018.06.20   JT Added SO.VersionID, PO.PurchaseOrderID, PO.VersionID 
   2018.06.25	NA Converted to ItemStock schema
   2018.10.16	NA Added InspectionType
   2018.10.17	NA Added VendorType
   Return Codes:
				
  
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspQCInspectionGet]
	@InspectionID INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	DECLARE @Sec TABLE (InspectionID INT, RoleID INT)
	INSERT @Sec EXECUTE uspQCInspectionSecurityGet @UserID = @UserID;

	SELECT 
		Q.InspectionID,
		SQ.StockID,
		SQ.Qty StockQty,
		I.ItemID,
		Q.QtyFailed,
		Q.InspectionQty,
		Q.CompletedBy,
		ISNULL(UC.FirstName + ' ' + UC.LastName,'') CompletedByUser,
		Q.CompletedDate,
		Q.CreatedBy,
		Q.Created,
		SQ.POLineID,
		SPA.Qty,
		SQ.DateCode,
		SQ.PackagingID,
		I.CommodityID,
		I.ItemStatusID,
		I.PartNumber,
		I.PartNumberStrip,
		M.MfrName,
		I.PartDescription,
		W.WarehouseName,
		SA.AccountName 'CustomerAccount',
		SA.AccountID 'CustomerAccountID',
		PA.AccountName 'VendorAccount',
		PA.AccountID 'VendorAccountID',
		CT.[Name] 'VendorType',
		SQ.ExternalID 'LotNumber',
		SQ.ReceivedDate 'ReceivedDate',
		SL.ShipDate 'ShipDate',
		SO. SalesOrderID,
		SO.VersionID 'SOVersionID',
		PO.PurchaseOrderID,
		PO.VersionID 'POVersionID',
		SQ.Qty 'ItemQty',
		SO.QCNotes, --QC Notes from SO
		Q.ExternalId,
		PO.ExternalId POExternalID,
		SO.ExternalID SOExternalID,
		Q.InspectionStatusID,
		Q.InspectionTypeID,
		IT.TypeName 'InspectionTypeName',
		Q.ResultID,
		QCR.ResultName,
		QCR.DecisionCode,
		QCR.AcceptanceCode,
		@UserID 'UserID'
	 FROM QCInspections Q
	INNER JOIN vwStockQty SQ on SQ.StockID = Q.StockID
	INNER JOIN vwPurchaseOrderLines PL on PL.POLineID = SQ.POLineID
	INNER JOIN vwPurchaseOrders PO on PO.PurchaseOrderID = PL.PurchaseOrderID
	LEFT OUTER JOIN mapSOInvFulfillment SPA on SPA.StockID = SQ.StockID
	LEFT OUTER JOIN vwSalesOrderLines SL on SL.SOLineID = SPA.SOLineID
	LEFT OUTER JOIN vwSalesOrders SO on SO.SalesOrderID = SL.SalesOrderID
	LEFT OUTER JOIN Items I ON I.ItemID = SQ.ItemID	
	LEFT OUTER JOIN Warehouses W on W.WarehouseID = SQ.WarehouseID
	LEFT OUTER JOIN Manufacturers M on M.MfrID = I.MfrID
	LEFT OUTER JOIN Accounts SA on SA.AccountID = SO.AccountID
	LEFT OUTER JOIN Accounts PA on pA.AccountID = PO.AccountID
	LEFT OUTER JOIN lkpCompanyTypes CT ON PA.CompanyTypeID = CT.CompanyTypeID
	LEFT OUTER JOIN Users UC on UC.UserID = Q.CompletedBy
	LEFT OUTER JOIN lkpQCResults QCR on Q.ResultID = QCR.ResultID
	INNER JOIN lkpQCInspectionTypes IT ON Q.InspectionTypeID = IT.InspectionTypeID
	INNER JOIN Users U on U.UserID = @UserID
	INNER JOIN (SELECT DISTINCT InspectionID FROM @Sec) sec ON Q.InspectionID = sec.InspectionID
	WHERE Q.InspectionID = ISNULL(@InspectionID, Q.InspectionID)
	IF (@@rowcount = 0)
		RETURN -1
END