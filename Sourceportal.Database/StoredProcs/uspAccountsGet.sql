/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.07
   Description:	Gets Accounts from Accounts tbl using AccountID to narrow
   Usage: EXEC uspAccountsGet @UserID=76 , @SearchString='#'
          EXEC uspAccountsGet @UserID=3, @SearchString='Flextronics'
          EXEC uspAccountsGet @AccountID = 3,@UserID=3
          EXEC uspAccountsGet @SortBy= AccountNum,@UserID=3,@RowLimit=25
		  EXEC uspAccountsGet @AccountID=1,@RowOffset=0,@RowLimit=0,@SortBy=NULL,@DescSort=0,@UserID=3

   Revision History:
		2017.06.27  NA  Replaced Accounts.Name with Accounts.AccountName
		2017.10.06 ML Added ExternalID
		2017.10.13 CT Added StatusExternalID
		2017.10.19 ML Remove ParentID
		2017.10.23 Julia T Added Owners,AccountType,AccountStatus,City,Country,Organization
		2017.10.23	AR	Added Contact/Parent Contact
		2017.10.24 Julia T Added sorting and paging   
		2017.10.27 Julia T Added SearingString 
		2017.11.10 NA	Fixed bug in fnGetObjectOwners call
		2017.11.28	BZ	Join with Security for ContactCount
		2017.11.29	BZ Updated WHERE clause, removed account id search, fixed fnGetObjectOwners
		2017.12.05	BZ	Added IsDeleted = 0 in WHERE, Added OpenBalance and CreditLimit
		2017.12.07	BZ	Added email, website, yearEstablished, numOfEmployees, EndProductFocus,
			CarryStock, MinimumPO, ShippingInstructions, VendorNum, SupplierRating
		2018.01.10  CT  Added Bitwise operation on LocationTypeID
		2018.01.22	CT	Removed Account Status, Updated Account Type to join with mapAccountTypes
		2018.02.06  RV  Added QCNotes and PO Notes to the account details page
		2018.02.20	BZ	Added IsDeleted to join condition with mapAccountTypes table
		2018.08.06  CT  Added IncotermID
		2018.08.29	NA	Added IsSourceability
   Return Codes:
   ============================================= */
CREATE   PROCEDURE [dbo].[uspAccountsGet]
(
	  @AccountID INT = NULL
	, @IsDeleted BIT = 0
	,@RowOffset INT =0
	,@RowLimit INT = 50
	,@SortBy NVARCHAR(25)=''
	,@DescSort BIT = 0
	,@SearchString NVARCHAR(32) = ''
	,@UserID INT = NULL
	
)
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @Sec TABLE (AccountID INT, RoleID INT)
	INSERT @Sec EXECUTE uspAccountSecurityGet @UserID = @UserID;

	DECLARE @ContactSec TABLE (ContactID INT, RoleID INT)
	INSERT @ContactSec EXECUTE uspContactSecurityGet @UserID = @UserID;

	WITH Main_Temp AS (
	  SELECT
	     A.AccountID
		, SUM(mat.AccountTypeID) AccountTypeBitwise --A.AccountTypeID AccountTypeBitwise
		, CompanyTypeID
		, A.OrganizationID
		, CurrencyID
		, mat.PaymentTermID
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
		, A.NumOfEmployees
		, A.ApprovedVendor
		, A.EndProductFocus
		, A.CarryStock
		, A.MinimumPO
		, A.ShippingInstructions
		, A.VendorNum
		, A.SupplierRating
		,dbo.fnGetObjectOwners(a.AccountID, 1) Owners
		, dbo.fnGetAccountTypes(a.AccountID) AccountType
		,L.City
		,C.CountryName
		,(O.Name) Organization
		--, CO.FirstName ContactFirstName
		, CC.TotalContactCount
		, A.QCNotes
		, A.PONotes 
		, A.IncotermID
		, A.DBNum
		, A.IsSourceability
		--, CO.ContactID
		--, PC.ParentContactID ParentContactID
		--, CASE WHEN ParentContactID = CO.ContactID THEN 1 ELSE 0 END IsParent
		FROM Accounts A
		INNER JOIN mapAccountTypes mat ON mat.AccountID = A.AccountID AND mat.IsDeleted = 0
		LEFT JOIN Locations L ON L.AccountID = A.AccountID AND L.LocationTypeID & 1 = 1 AND L.IsDeleted = 0 --BillTo Address 
		LEFT JOIN Countries C ON C.CountryID = L.CountryID
		INNER JOIN (SELECT DISTINCT AccountID FROM @Sec) sec ON A.AccountID = sec.AccountID
		OUTER APPLY (
			SELECT 
				COUNT(CO.AccountID) TotalContactCount
			FROM Contacts CO 
				INNER JOIN (SELECT DISTINCT ContactID FROM @ContactSec) csec ON CO.ContactID = csec.ContactID
			WHERE CO.IsDeleted = 0 AND CO.AccountID = A.AccountID GROUP BY CO.AccountID) CC
		LEFT OUTER JOIN Organizations O ON O.OrganizationID = A.OrganizationID
		WHERE A.AccountID = ISNULL(@AccountID, A.AccountID)
		AND A.IsDeleted = ISNULL(@IsDeleted, A.IsDeleted)
		AND (ISNULL(A.AccountName,'') + ISNULL(dbo.fnGetAccountTypes(a.AccountID),'') + ISNULL(A.AccountNum,'')+ISNULL(C.CountryName,'') + ISNULL(L.City,'') + ISNULL(O.[Name],'')+ ISNULL(dbo.fnGetObjectOwners(a.AccountID, dbo.fnAccountObjectTypeID()),'') LIKE '%'+ISNULL(@SearchString,'')+ '%' )

		GROUP BY A.AccountID, CompanyTypeID
		, A.OrganizationID
		, CurrencyID
		, PaymentTermID
		, AccountName 
		, AccountNum
		, A.Created
		, A.ApprovedVendor
		, A.CreatedBy
		, A.IsDeleted
		, A.ExternalID
		, A.AccountHierarchyID
		, A.CreditLimit
		, A.OpenBalance
		, A.Email
		, A.Website
		, A.YearEstablished
		, A.NumOfEmployees
		, A.EndProductFocus
		, A.CarryStock
		, A.MinimumPO
		, A.ShippingInstructions
		, A.VendorNum
		, A.SupplierRating
		,L.City
		,C.CountryName
		,O.Name
		, CC.TotalContactCount 
		, A.QCNotes
		, A.PONotes 
		, A.IncotermID
		, A.DBNum
		, A.IsSourceability
	),
	Count_Temp AS(
	    SELECT COUNT(*) AS TotalRowCount
		From Main_Temp
	     )


	SELECT *
	FROM Main_Temp,Count_Temp

	ORDER BY 
	CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.AccountID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName 
				WHEN @SortBy = 'AccountNum' THEN Main_Temp.AccountNum
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountType' THEN Main_Temp.AccountType
				WHEN @SortBy = 'Country' THEN Main_Temp.CountryName
				WHEN @SortBy = 'City' THEN Main_Temp.City
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'Organization' THEN Main_Temp.Organization 
				WHEN @SortBy = 'Owner' THEN Main_Temp.Owners
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'Contacts' THEN Main_Temp.TotalContactCount
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.AccountID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName 
				WHEN @SortBy = 'AccountNum' THEN Main_Temp.AccountNum
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountType' THEN Main_Temp.AccountType
				WHEN @SortBy = 'Country' THEN Main_Temp.CountryName
				WHEN @SortBy = 'City' THEN Main_Temp.City
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'Organization' THEN Main_Temp.Organization 
				WHEN @SortBy = 'Owner' THEN Main_Temp.Owners
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'Contacts' THEN Main_Temp.TotalContactCount
			END
		END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT NULLIF(@RowLimit,0) ROWS ONLY
		
END