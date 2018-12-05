/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.07
   Description:	Gets Accounts from Accounts tbl using AccountID to narrow
   Usage: EXEC [uspQuoteReportHeaderGet] @QuoteID=100007, @VersionID = 1, @UserID = 1

   Revision History:

   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteReportHeaderGet]
(
	  @QuoteID INT = NULL,
	  @VersionID INT = 1,
	  @IsDeleted BIT = 0,
	  @UserID INT
)
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @Sec TABLE (AccountID INT, RoleID INT)
	INSERT @Sec EXECUTE uspAccountSecurityGet @UserID = @UserID;
	
	--Get Initial Values
	DECLARE @AccountID INT, @SalesPerson VARCHAR(128), @SentDate VARCHAR(64)
		SELECT TOP 1 @AccountID = AccountID
		, @SalesPerson = dbo.fnGetObjectOwners(19, @QuoteID)
		, @SentDate = CONVERT(VARCHAR(10), Q.Created,126)
		FROM Quotes Q
		WHERE QuoteID = @QuoteID AND versionID = @VersionID
	--Select/Return All

	  SELECT
	     A.AccountID
		, @QuoteID QuoteID
		, @SalesPerson SalesPerson
		, @SentDate SentDate
		, SUM(mat.AccountTypeID) AccountTypeBitwise
		, CompanyTypeID
		, A.OrganizationID
		--, A.AccountStatusID
		--, S.ExternalID StatusExternalID
		--, CurrencyID
		, AccountName 
		, AccountNum
		, A.Created
		, A.CreatedBy
		, A.IsDeleted
		, A.ExternalID
		, A.AccountHierarchyID
		, A.CreditLimit
		, A.OpenBalance
		, A.Email
		, A.Website
		, A.YearEstablished
		, A.CarryStock
		, A.MinimumPO
		, A.ShippingInstructions
		, A.VendorNum
		, A.SupplierRating
		--,dbo.fnGetObjectOwners(a.AccountID, 1) Owners
		--, dbo.fnGetAccountTypes(a.AccountID) AccountType
		--,(s.Name) AccountStatus
		, L.Address1
		, L.City
		, S.StateName
		, C.CountryName
		, L.PostalCode
		, L.City + ', ' + S.StateCode + ' ' + L.PostalCode CityStatePostal
		FROM Accounts A
		--INNER JOIN lkpAccountStatuses S ON S.AccountStatusID = A.AccountStatusID
		INNER JOIN mapAccountTypes mat ON mat.AccountID = A.AccountID
		INNER JOIN Locations L ON L.AccountID = A.AccountID AND L.LocationTypeID & 1 = 1 AND L.IsDeleted = 0 --BillTo Address 
		INNER JOIN Countries C ON C.CountryID = L.CountryID
		LEFT OUTER JOIN States S on S.StateID = L.StateID
		INNER JOIN (SELECT DISTINCT AccountID FROM @Sec) sec ON A.AccountID = sec.AccountID
		WHERE A.AccountID = @AccountID
		--LEFT OUTER JOIN Contacts CO on CO.AccountID = A.AccountID
		--OUTER APPLY (SELECT TOP 1 ContactID ParentContactID FROM Contacts PC WHERE PC.AccountID = A.AccountID) PC
		
		GROUP BY 
		 A.AccountID
		--, SUM(mat.AccountTypeID) 
		, CompanyTypeID
		, A.OrganizationID
		, AccountName 
		, AccountNum
		, A.Created
		, A.CreatedBy
		, A.IsDeleted
		, A.ExternalID
		, A.AccountHierarchyID
		, A.CreditLimit
		, A.OpenBalance
		, A.Email
		, A.Website
		, A.YearEstablished
		, A.CarryStock
		, A.MinimumPO
		, A.ShippingInstructions
		, A.VendorNum
		, A.SupplierRating
		, L.Address1
		, L.City
		, S.StateName
		, C.CountryName
		, L.PostalCode
		, L.City + ', ' + S.StateCode + ' ' + L.PostalCode
		
END
