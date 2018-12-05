/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.24
   Description:	Retrieves a list of all open sales orders in the system
   Usage:		EXEC uspSalesOrdersListGet
   Return Codes:
				
   Revision History:
		2017.12.12		BZ		Fix owners get
		2018.02.05		NA		Changed Location join to Bitwise
		2018.10.05		NA		Modified ORDER BY logic

   ============================================= */


CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrdersListGet]
	@SalesOrderID INT = NULL,
	@SearchString NVARCHAR(100) = '',
	@IncludeComplete BIT = 0,
	@IncludeCanceled BIT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@UserID INT = NULL
AS
BEGIN
	DECLARE @Sec TABLE (SalesOrderID INT, RoleID INT)
	INSERT @Sec EXECUTE uspSalesOrderSecurityGet @UserID = @UserID;
	WITH Main_CTE AS (
		SELECT 
			so.SalesOrderID,
			so.VersionID,
			so.AccountID,
			a.AccountName,
			so.ContactID,
			c.FirstName 'ContactFirstName',
			c.LastName 'ContactLastName',
			so.StatusID,
			s.StatusName,
			ct.CountryName,
			so.OrganizationID,
			org.[Name] 'OrganizationName',
			so.OrderDate,
			so.ExternalID,
			dbo.fnGetObjectOwners(so.SalesOrderID, 16) Owners --16 = Sales Order (lkpObjectTypes)
		FROM vwSalesOrders so 
		  LEFT OUTER JOIN Accounts AS a ON so.AccountID = a.AccountID
		  LEFT OUTER JOIN Contacts AS c ON so.ContactID = c.ContactID
		  LEFT OUTER JOIN lkpStatuses AS s ON so.StatusID = s.StatusID
		  LEFT OUTER JOIN Organizations AS org ON so.OrganizationID = org.OrganizationID
		  INNER JOIN Locations l ON l.AccountID = a.AccountID AND l.LocationTypeID & 1 = 1 --Bill-To
		  INNER JOIN Countries ct ON ct.CountryID = l.CountryID
		  INNER JOIN (SELECT DISTINCT SalesOrderID FROM @Sec) sec ON so.SalesOrderID = sec.SalesOrderID
		WHERE ISNULL(s.IsComplete, 0) IN(0, @IncludeComplete)
		  AND ISNULL(s.IsCanceled, 0) IN(0, @IncludeCanceled)
		  AND (dbo.fnReturnOrderDisplayID(so.SalesOrderID, so.ExternalID) + ISNULL(a.AccountName, '') + ISNULL(c.FirstName, '') + ISNULL(c.LastName, '') + ISNULL(c.FirstName, '') + ' ' + ISNULL(c.LastName, '') + ISNULL(dbo.fnGetObjectOwners(so.SalesOrderID, 16), '') LIKE '%' + ISNULL(@SearchString,'') + '%')
	),
	Count_CTE AS (
		SELECT COUNT(*) 'TotalRows'
		FROM Main_CTE
	)
	SELECT * FROM Main_CTE m, Count_CTE
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'OrderNum' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN m.AccountName
				WHEN @SortBy = 'ContactFirstName' THEN m.ContactFirstName + m.ContactLastName
				WHEN @SortBy = 'StatusName' THEN m.StatusName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE 
				WHEN @SortBy = 'CustomerCountry' THEN m.CountryName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE 
				WHEN @SortBy = 'OrderDate' THEN m.OrderDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE 
				WHEN @SortBy = 'Owner' THEN dbo.fnGetObjectOwners(m.SalesOrderID, 16)
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'OrderNum' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN m.AccountName
				WHEN @SortBy = 'ContactFirstname' THEN m.ContactFirstName + m.ContactLastName
				WHEN @SortBy = 'StatusName' THEN m.StatusName				
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE 
				WHEN @SortBy = 'CustomerCountry' THEN m.CountryName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE 
				WHEN @SortBy = 'OrderDate' THEN m.OrderDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE 
				WHEN @SortBy = 'Owner' THEN dbo.fnGetObjectOwners(m.SalesOrderID, 16)
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END
