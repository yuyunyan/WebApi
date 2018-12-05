/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.04
   Description:	Gets details for purchase order report
   Usage: EXEC [uspPurchaseOrderReportDetailsGet] @PurchaseOrderID=100009, @VersionID = 2

   Revision History:

   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspPurchaseOrderReportDetailsGet]
(
	  @PurchaseOrderID INT = NULL,
	  @VersionID INT = 1
)
AS
BEGIN 
	SET NOCOUNT ON;
	SELECT TOP 1 AccountNum
		, P.PurchaseOrderID
		, dbo.fnReturnOrderDisplayID(P.PurchaseOrderID, P.ExternalID) [DisplayID]
		, PT.TermName [Terms]
		, SM.MethodName [Ship Via]
		,  IT.IncotermName [Incoterms]
		--, NULL [Reference #]
		, C.FirstName + ' ' + C.LastName AttentionTo
		, C.OfficePhone [Phone #]
		, SO.FreightPaymentID FreightService
		, SO.FreightAccount FreightAcct
		, P.PaymentTermID
		--, CONVERT(VARCHAR(50), P.ValidForHours/24) + ' Days' [Quote Valid Through]
		--, CONVERT(VARCHAR(10), DATEADD(HOUR, ValidForHours,  Q.SentDate),126) [Quote Valid Through]
		, CONVERT(CHAR(15), P.Created, 106) CreatedDateFormatted
		--, P.ValidForHours
		, FORMAT(GETUTCDATE(), 'dd MMM yyy') [Date]
		, p.PONotes PONotes
		, a.CreatedBy
		, dbo.fnGetUserFullName(p.CreatedBy) CreatedByName
		FROM PurchaseOrders P
	INNER JOIN dbo.Contacts C on C.ContactID = P.ContactID
	INNER JOIN dbo.Accounts A on A.AccountID = P.AccountID
	LEFT OUTER JOIN (SELECT TOP 1 L.PurchaseOrderID
						, L.POVersionID
						, S.FreightAccount
						, S.FreightPaymentID
					FROM PurchaseOrderLines L
					INNER JOIN mapSOPOAllocation SP on SP.POLineID = L.POLineID
					INNER JOIN SalesOrderLines SL on SL.SOLineID = SP.POLineID
					INNER JOIN SalesOrders S on S.SalesOrderID = SL.SalesOrderID ) SO
					ON SO.PurchaseOrderID = @PurchaseOrderID AND SO.POVersionID = @VersionID
	LEFT OUTER JOIN [codes].[lkpPaymentTerms] PT on PT.PaymentTermID = P.PaymentTermID
	LEFT OUTER JOIN [codes].[lkpShippingMethods] SM on SM.ShippingMethodID = P.ShippingMethodID
	LEFT OUTER JOIN [codes].[lkpIncoTerms] IT on IT.IncoTermID = P.IncoTermID
	--LEFT OUTER JOIN dbo.[SalesOrders] SO on SO.QuoteID = P.PurchaseOrder
	WHERE P.PurchaseOrderID = @PurchaseOrderID
	AND P.VersionID = @VersionID
END