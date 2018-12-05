/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.20
   Description:	Retrieves the header information for a given quote & version
   Usage:		EXEC uspQuoteGet 100007, 1, 1
   Return Codes:
				-1 Missing QuoteID or VersionID
   Revision History:
				2017.06.21  NA  Removed Ownership result, moving to separate procedure
				2017.06.21  NA  Added ShipLocationID
				2017.06.27  NA  Replaced Account.Name with Account.AccountName
				2017.10.24	BZ	Added Security
				2017.11.06  CT  Added SAP Fields - Organization, Incoterms, PaymentTerms, Currencies
				2018.01.08	AR	Added Owners, UserID, SentDate columns
				2018.02.06  CT  Added IncotermLocation to select
				2018.04.05	BZ	Added QuoteType
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteGet]
	@QuoteID INT = NULL,
	@VersionID INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@QuoteID, 0) = 0 OR ISNULL(@VersionID, 0) = 0
		RETURN -1

	DECLARE @QuotePrice MONEY
	DECLARE @QuoteCost MONEY
	DECLARE @QuoteProfit MONEY
	DECLARE @QuoteGPM FLOAT

	DECLARE @Sec TABLE (QuoteID INT, RoleID INT)
	INSERT @Sec EXECUTE uspQuoteSecurityGet @UserID = @UserID;

	SELECT	@QuotePrice = ROUND(SUM(Qty * Price), 2),
			@QuoteCost = ROUND(SUM(Qty * Cost), 2),
			@QuoteProfit = ROUND(SUM(Qty * Price - Qty * Cost), 2),
			@QuoteGPM = CASE WHEN SUM(Qty * Price) <> 0 THEN ROUND((SUM(Qty * Price) - SUM(Qty * Cost)) / SUM(Qty * Price), 4) ELSE 0 END
	FROM QuoteLines
	WHERE QuoteID = @QuoteID AND QuoteVersionID = @VersionID

	SELECT	q.QuoteID
			, q.VersionID
			, q.StatusID
			, s.StatusName
			, q.AccountID
			, a.AccountName
			, q.ContactID
			, q.ShipLocationID
			, c.FirstName + ' ' + c.LastName 'ContactName'
			, c.OfficePhone
			, c.Email
			, q.ValidForHours
			, q.OrganizationID
			, q.QuoteTypeID
			--, qt.QuoteTypeID
			--, qt.TypeName
			, o.ExternalID 'OrganizationExternalID'
			, q.IncotermID
			, i.ExternalID 'IncotermExternalID'
			, q.PaymentTermID
			, p.ExternalID 'PaymentTermExternalID'
			, q.CurrencyID
			, cur.ExternalID 'CurrencyExternalID'
			, q.ShippingMethodID
			, ISNULL(@QuotePrice, 0) 'QuotePrice'
			, ISNULL(@QuoteCost, 0) 'QuoteCost'
			, ISNULL(@QuoteProfit, 0) 'QuoteProfit'
			, ISNULL(@QuoteGPM, 0) 'QuoteGPM'
			, q.IncotermLocation			
			, CONVERT(CHAR(15), q.Created, 106) Created
			, q.IsDeleted
			, OW.FullName Salesperson
			, OW.EmailAddress SalesPersonEmail
			, CONVERT(VARCHAR(10),Q.SentDate,126) SentDate
			, @UserID UserID
	FROM Quotes q
	  --LEFT OUTER JOIN lkpQuoteTypes qt on q.QuoteTypeID = qt.QuoteTypeID
	  LEFT OUTER JOIN lkpStatuses s ON q.StatusID = s.StatusID
	  LEFT OUTER JOIN Accounts a ON q.AccountID = a.AccountID
	  LEFT OUTER JOIN Contacts c ON q.ContactID = c.ContactID
	  LEFT OUTER JOIN Organizations o ON q.OrganizationID = o.OrganizationID
	  LEFT OUTER JOIN codes.lkpIncoterms i ON q.IncotermID = i.IncotermID
	  LEFT OUTER JOIN codes.lkpPaymentTerms p ON q.PaymentTermID = p.PaymentTermID
	  LEFT OUTER JOIN lkpCurrencies cur ON q.CurrencyID = cur.CurrencyID
	  INNER JOIN (SELECT DISTINCT QuoteID FROM @Sec) sec ON q.QuoteID = sec.QuoteID
	  LEFT OUTER JOIN (SELECT TOP 1 U.FirstName + ' ' + U.LastName FullName
						, U.EmailAddress
						, O.ObjectID
						FROM mapOwnership O
						INNER JOIN Users U on U.UserID = O.OwnerID
						WHERE ObjectTypeID = 19 AND ObjectID = @QuoteID ORDER BY [Percent] DESC) OW on OW.ObjectID = @QuoteID
	WHERE q.QuoteID = @QuoteID 
	  AND q.VersionID = @VersionID
END
