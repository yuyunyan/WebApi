/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.23
   Description:	Retrieves a list of all open quotes in the system
   Usage:		EXEC uspQuotesListGet
   Return Codes:
				
   Revision History:
			2017.06.27  NA  Replaced Accounts.Name with Accounts.AccountName
			2017.07.18	AR	Added Owners, CountryName to select statement
			2017.07.19	NA	Implemented vwQuotes
			2017.07.19	AR	Added ISNULL to @SearchString
			2018.02.01  RV  Added sorting and filtering to quotes
			2018.02.02	NA	Changed location type join to bitwise
   ============================================= */


CREATE PROCEDURE [dbo].[uspQuotesListGet]
	@QuoteID INT = NULL,
	@SearchString NVARCHAR(100) = '',
	@IncludeComplete BIT = 0,
	@IncludeCanceled BIT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@FilterBy NVARCHAR(25) = '',
	@FilterText VARCHAR(256) = '',
	@UserID INT = NULL
AS
BEGIN
	DECLARE @Sec TABLE (QuoteID INT, RoleID INT)
	INSERT @Sec EXECUTE uspQuoteSecurityGet @UserID = @UserID;
		
	WITH Main_CTE AS (
		SELECT 
			q.QuoteID,
			q.VersionID,
			q.AccountID,
			a.AccountName,
			q.ContactID,
			c.FirstName 'ContactFirstName',
			c.LastName 'ContactLastName',
			q.StatusID,
			s.StatusName,
			dbo.fnGetObjectOwners(q.QuoteID, 19) Owners, --19 = Quote (lkpObjectTypes)
			CT.CountryName,
			q.OrganizationID,
			q.SentDate			
		FROM vwQuotes q 
		  LEFT OUTER JOIN Accounts AS a ON q.AccountID = a.AccountID
		  LEFT OUTER JOIN Contacts AS c ON q.ContactID = c.ContactID
		  LEFT OUTER JOIN lkpStatuses AS s ON q.StatusID = s.StatusID
		  INNER JOIN Locations L on L.AccountID = A.AccountID AND L.LocationTypeID & 1 = 1 --Bill-To
		  INNER JOIN Countries CT on CT.CountryID = L.CountryID
		  INNER JOIN (SELECT DISTINCT QuoteID FROM @Sec) sec ON q.QuoteID = sec.QuoteID
		WHERE ISNULL(s.IsComplete, 0) IN(0, @IncludeComplete)
		  AND ISNULL(s.IsCanceled, 0) IN(0, @IncludeCanceled)
		  AND (CAST(q.QuoteID AS NVARCHAR(16)) + ISNULL(a.AccountName, '')+ ISNULL(s.StatusName, '') + ISNULL(dbo.fnGetObjectOwners(q.QuoteID, 19), '') + ISNULL(c.FirstName, '') + ISNULL(c.LastName, '') LIKE '%' + ISNULL(@SearchString,'') + '%')
		  AND (@FilterBy IS NULL OR (
                     ((@FilterBy = 'ContactFirstName' AND c.FirstName LIKE '%' + ISNULL(@FilterText , '') + '%')
                     OR
                     (@FilterBy = 'LastName' AND LastName LIKE '%' + ISNULL(@FilterText , '') + '%')
                     OR 
					  (@FilterBy = 'AccountName' AND AccountName LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR
						(@FilterBy = 'StatusName' AND s.StatusName LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR
						(@FilterBy = 'CountryName' AND CT.CountryName LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR
						(@FilterBy = 'Owners' AND dbo.fnGetObjectOwners(q.QuoteID, 19) LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR 
						(@FilterBy = 'SentDate' AND q.SentDate LIKE '%' + ISNULL(@FilterText , '') + '%')
              )))
	),
	Count_CTE AS (
		SELECT COUNT(*) 'TotalRows'
		FROM Main_CTE
	)

	SELECT * FROM Main_CTE m, Count_CTE
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN m.QuoteID
				WHEN @SortBy = 'QuoteID' THEN m.QuoteID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN m.AccountName
				WHEN @SortBy = 'ContactFirstName' THEN m.ContactFirstName
				WHEN @SortBy = 'ContactLastName' THEN m.ContactLastName
				WHEN @SortBy = 'StatusName' THEN m.StatusName	
				WHEN @SortBy = 'CountryName' THEN m.countryName
				WHEN @SortBy = 'Owners' THEN dbo.fnGetObjectOwners(m.QuoteID, 19)	
					
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'SentDate' THEN m.SentDate 	
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN m.QuoteID
				WHEN @SortBy = 'QuoteID' THEN m.QuoteID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN m.AccountName
				WHEN @SortBy = 'ContactFirstName' THEN m.ContactFirstName
				WHEN @SortBy = 'ContactLastName' THEN m.ContactLastName
				WHEN @SortBy = 'StatusName' THEN m.StatusName	
				WHEN @SortBy = 'CountryName' THEN m.countryName
				WHEN @SortBy = 'Owners' THEN dbo.fnGetObjectOwners(m.QuoteID, 19)	
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'SentDate' THEN m.SentDate 
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END