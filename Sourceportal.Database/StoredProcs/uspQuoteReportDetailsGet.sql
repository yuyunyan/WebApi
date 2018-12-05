
/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.12.27
   Description:	Gets details for quote report
   Usage: EXEC [uspQuoteReportDetailsGet] @QuoteID=100007, @VersionID = 6 --, @UserID = 1

   Revision History:

   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteReportDetailsGet]
(
	  @QuoteID INT = NULL,
	  @VersionID INT = 1,
	  @IsDeleted BIT = 0
	  --@UserID INT
)
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @Owners VARCHAR(500) = (SELECT dbo.fnGetObjectOwners(@QuoteID, 19))
	SELECT TOP 1 AccountNum
		, Q.QuoteID
		, PT.TermName [Terms]
		, SM.MethodName [Ship Via]
		,  IT.IncotermName [Incoterms]
		--, NULL [Reference #]
		, C.FirstName + ' ' + C.LastName [Buyer]
		, C.OfficePhone [Phone #]
		, Q.PaymentTermID
		, CONVERT(VARCHAR(50), Q.ValidForHours/24) + ' Days' [Quote Valid Through]
		--, CONVERT(VARCHAR(10), DATEADD(HOUR, ValidForHours,  Q.SentDate),126) [Quote Valid Through]
		, Q.SentDate
		, Q.ValidForHours
		, LEFT(@Owners, LEN(@Owners)-1) Owners
		, FORMAT(GETUTCDATE(), 'dd MMM yyy') [Date]
		FROM Quotes Q
	INNER JOIN dbo.Contacts C on C.ContactID = Q.ContactID
	INNER JOIN dbo.Accounts A on A.AccountID = Q.AccountID
	LEFT OUTER JOIN [codes].[lkpPaymentTerms] PT on PT.PaymentTermID = Q.PaymentTermID
	LEFT OUTER JOIN [codes].[lkpShippingMethods] SM on SM.ShippingMethodID = Q.ShippingMethodID
	LEFT OUTER JOIN [codes].[lkpIncoTerms] IT on IT.IncoTermID = Q.IncoTermID
	LEFT OUTER JOIN dbo.[SalesOrders] SO on SO.QuoteID = Q.QuoteID
	WHERE Q.QuoteID = @QuoteID
	AND Q.VersionID = @VersionID
		
END
