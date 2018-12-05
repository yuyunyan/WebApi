/* =============================================
   Author:		Remya Varriem
   Create date: 2018.02.02
   Description:	Gets Accounts from Accounts tbl using AccountID to narrow
   Usage: EXEC uspAccountsGetByType @UserID=76 , @SearchString='#'
		  EXEC uspAccountsGetByType @UserID=76, @AccountTypeID = 1 , @AccountIsActive = 1
          EXEC uspAccountsGetByType @UserID=76, @AccountTypeID = 4 , @AccountIsActive = 0
		  EXEC uspAccountsGetByType @UserID=76, @AccountTypeID = 0
		  EXEC uspAccountsGetByType @RowOffset=0,@RowLimit=20,@SortBy=N'',@DescSort=0,@SearchString=N'',@AccountTypeId=4,@FilterBy=NULL,@FilterText=NULL,@AccountIsActive=1,@UserID=1

   Revision History:
		2018.02.02  RV  Added AccountTypeID to get all accounts
		2018.02.27	BZ	Added IsDeleted to join conditon with mapAccountType table
		2018.07.19	AR	Removed contacts count outer apply & removed contactsecurity proc

   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountsGetByType]
(
	  @AccountID INT = NULL
	, @IsDeleted BIT = 0
	,@RowOffset INT =0
	,@RowLimit INT = 100
	,@SortBy NVARCHAR(25)=''
	,@DescSort BIT = 0
	,@SearchString NVARCHAR(32) = ''
	,@UserID INT = NULL
	,@FilterBy NVARCHAR(25) = NULL
	,@FilterText VARCHAR(256) = NULL
	,@AccountTypeID INT = 0
	,@AccountIsActive BIT = 1
	
)
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @Sec TABLE (AccountID INT, RoleID INT)
	INSERT @Sec EXECUTE uspAccountSecurityGet @UserID = @UserID;

	--DECLARE @ContactSec TABLE (ContactID INT, RoleID INT)
	--INSERT @ContactSec EXECUTE uspContactSecurityGet @UserID = @UserID;

	WITH Main_Temp AS (
	  SELECT
	     A.AccountID
		 , SUM(mat.AccountTypeID) AccountTypeBitwise --A.AccountTypeID AccountTypeBitwise
		, CompanyTypeID
		, A.OrganizationID
		, CurrencyID
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
		, A.EndProductFocus
		, A.CarryStock
		, A.MinimumPO
		, A.ShippingInstructions
		, A.VendorNum
		, A.SupplierRating
		,dbo.fnGetObjectOwners(a.AccountID, 1) Owners
		,L.City
		,C.CountryName
		,(O.Name) Organization
		--, CC.TotalContactCount 
		, (s.Name) AccountStatus
		, s.AccountIsActive
		FROM Accounts A
		INNER JOIN mapAccountTypes mat ON mat.AccountID = A.AccountID AND mat.IsDeleted = 0
		INNER JOIN lkpAccountTypes T on T.AccountTypeID & mat.AccountTypeID = T.AccountTypeID
		LEFT JOIN lkpAccountStatuses s on mat.AccountStatusID = s.AccountStatusID 
		--and mat.AccountTypeID = @AccountTypeID 
		LEFT JOIN Locations L ON L.AccountID = A.AccountID AND L.LocationTypeID & 1 = 1 AND L.IsDeleted = 0 --BillTo Address 
		LEFT JOIN Countries C ON C.CountryID = L.CountryID
		INNER JOIN (SELECT DISTINCT AccountID FROM @Sec) sec ON A.AccountID = sec.AccountID
		--OUTER APPLY (
		--	SELECT 
		--		COUNT(CO.AccountID) TotalContactCount
		--	FROM Contacts CO 
		--		INNER JOIN (SELECT DISTINCT ContactID FROM @ContactSec) csec ON CO.ContactID = csec.ContactID
		--	WHERE CO.IsDeleted = 0 AND CO.AccountID = A.AccountID GROUP BY CO.AccountID) CC
		LEFT OUTER JOIN Organizations O ON O.OrganizationID = A.OrganizationID
		WHERE A.AccountID = ISNULL(@AccountID, A.AccountID)
		AND A.IsDeleted = ISNULL(@IsDeleted, A.IsDeleted) 
		AND ((@AccountTypeID = 0) OR (T.AccountTypeID =ISNULL(@AccountTypeID, T.AccountTypeID) ) )
		AND ((@AccountIsActive = 1 AND s.AccountIsActive = ISNULL(@AccountIsActive,s.AccountIsActive)
				OR
			 (@AccountIsActive = 0 AND s.AccountIsActive IN (1 , 0 )) ) ) --return that are in either of the status
		AND (ISNULL(A.AccountName,'') + ISNULL(A.AccountNum,'') + ISNULL(dbo.fnGetObjectOwners(a.AccountID, dbo.fnAccountObjectTypeID()),'') LIKE '%'+ISNULL(@SearchString,'')+ '%' )
		   AND (@FilterBy IS NULL OR (
                     (@FilterBy = 'AccountNum' AND A.AccountNum LIKE '%' + ISNULL(@FilterText , '') + '%')
                     OR 
					  (@FilterBy = 'AccountName' AND A.AccountName LIKE '%' + ISNULL(@FilterText , '') + '%')
					    OR 
					  (@FilterBy = 'AccountStatus' AND s.[Name] LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR 
						(@FilterBy = 'Country' AND C.CountryName LIKE '%' + ISNULL(@FilterText , '') + '%')
						 OR 
						(@FilterBy = 'Organization' AND O.[Name] LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR
						 (@FilterBy = 'City' AND L.City LIKE '%' + ISNULL(@FilterText , '') + '%')
					OR
						(@FilterBy = 'Owner' AND dbo.fnGetObjectOwners(A.AccountID, dbo.fnAccountObjectTypeID()) LIKE '%' + ISNULL(@FilterText , '') + '%')
              ))

		GROUP BY A.AccountID, CompanyTypeID
		, A.OrganizationID
		, CurrencyID
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
		, A.EndProductFocus
		, A.CarryStock
		, A.MinimumPO
		, A.ShippingInstructions
		, A.VendorNum
		, A.SupplierRating
		,L.City
		,C.CountryName
		,O.Name
		--, CC.TotalContactCount
		, (s.Name) 
		, s.AccountIsActive
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
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.AccountName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName 
				WHEN @SortBy = 'AccountNum' THEN Main_Temp.AccountNum
				WHEN @SortBy = 'AccountStatus' THEN Main_Temp.AccountStatus
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
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
		--CASE WHEN @DescSort = 0 THEN
		--	CASE
		--		WHEN @SortBy = 'Contacts' THEN Main_Temp.TotalContactCount
		--	END
		--END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.AccountID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName 
				WHEN @SortBy = 'AccountNum' THEN Main_Temp.AccountNum 
				WHEN @SortBy = 'AccountStatus' THEN Main_Temp.AccountStatus
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Country' THEN Main_Temp.CountryName
				WHEN @SortBy = 'City' THEN Main_Temp.City
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'Organization' THEN Main_Temp.Organization 
				WHEN @SortBy = 'Owner' THEN Main_Temp.Owners
			END
		END DESC--,
		--CASE WHEN @DescSort = 1 THEN
		--	CASE
		--		WHEN @SortBy = 'Contacts' THEN Main_Temp.TotalContactCount
		--	END
		--END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT NULLIF(@RowLimit,0) ROWS ONLY
		
END
