/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.14
   Description:	Gets details for sales order report
   Usage: EXEC [uspSalesOrderReportDetailsGet] @SalesOrderID=100007, @VersionID = 2

   Revision History:

   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspSalesOrderReportDetailsGet]
(
	  @SalesOrderID INT = NULL,
	  @VersionID INT = 1
)
AS
BEGIN
	DECLARE @Owners VARCHAR(500) = dbo.fnGetObjectOwners(@SalesOrderID, 16)

	DECLARE @InvoiceID INT = 0
	EXEC @InvoiceID = dbo.uspLogReportGeneratedIns 'Proforma Invoice'

	SET NOCOUNT ON;
	SELECT TOP 1 S.SalesOrderID
		, S.StatusID
		, ST.StatusName
		, S.AccountID
		, A.AccountName
		, A.AccountNum
		, S.FreightAccount
		, S.CustomerPO
		, FORMAT(S.Created, 'dd MMM yyy') [Date]
		, S.CustomerPO CustomerPurchaseOrder
		, dbo.fnReturnOrderDisplayID(@SalesOrderID, S.ExternalID) DisplayID
		, PT.TermName PaymentTerm
		, C.FirstName + ' ' + C.LastName Buyer
		, L.[Name] + ' ' +  L.Address1 BillTo
		, SM.MethodName ShippingMethod
		, FP.MethodName FreightMethod
		, cm.MethodID 'CarrierMethodID'
		, s.FreightAccount
		, s.CarrierID
		, S.IncotermID
		, IT.IncotermName
		, S.IncotermLocation
		, LEFT(@Owners, LEN(@Owners)-1) Owners
		, S.ShippingNotes
		, @InvoiceID InvoiceID --TBC: SAP
		--, FORMAT(GETUTCDATE(), 'dd MMM yyy') [Date]
		--, dbo.fnGetUserFullName(p.CreatedBy) CreatedByName
		FROM SalesOrders S
	LEFT OUTER JOIN dbo.Contacts C on C.ContactID = S.ContactID
	LEFT OUTER JOIN dbo.Accounts A on A.AccountID = S.AccountID
	LEFT OUTER JOIN Locations L on L.LocationID = S.ShipLocationID
	LEFT OUTER JOIN [codes].[lkpPaymentTerms] PT on PT.PaymentTermID = S.PaymentTermID
	LEFT OUTER JOIN [codes].[lkpShippingMethods] SM on SM.ShippingMethodID = S.ShippingMethodID
	LEFT OUTER JOIN [codes].[lkpIncoTerms] IT on IT.IncoTermID = S.IncoTermID
	LEFT OUTER JOIN [codes].[lkpFreightPaymentMethods] FP on FP.FreightPaymentMethodID = S.FreightPaymentID
	LEFT OUTER JOIN CarrierMethods cm ON s.CarrierMethodID = cm.MethodID
	LEFT OUTER JOIN lkpStatuses st ON s.StatusID = st.StatusID
	WHERE S.SalesOrderID = @SalesOrderID
	AND S.VersionID = @VersionID
END