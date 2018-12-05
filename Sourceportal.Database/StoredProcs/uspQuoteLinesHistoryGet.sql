/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.08.24
				Returns Quote Line history by ItemID
   Usage:		EXEC uspQuoteLinesHistoryGet @ItemID = 78, @UserID = 64
   Return Codes:
   Revision History:
		2018.08.29	NA	Added GPM Calculation
		2018.10.16	NA	Added Account and Contact parameters
============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspQuoteLinesHistoryGet]
	@ItemID INT = NULL,
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@StartDate DATE = NULL,
	@EndDate DATE = NULL,
	@UserID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 1
AS
BEGIN
	IF @StartDate IS NULL
		SET @StartDate = DATEADD(month, -6, GETDATE())
	IF @EndDate IS NULL
		SET @EndDate = GETDATE()

	DECLARE @AccountSec TABLE (AccountID INT, RoleID INT)
	INSERT @AccountSec EXECUTE uspAccountSecurityGet @UserID = @UserID;

	DECLARE @ContactSec TABLE (ContactID INT, RoleID INT)
	INSERT @ContactSec EXECUTE uspContactSecurityGet @UserID = @UserID;

	DECLARE @QuoteSec TABLE (QuoteID INT, RoleID INT)
	INSERT @QuoteSec EXECUTE uspQuoteSecurityGet @UserID = @UserID;

	DECLARE @PartNumberStrip VARCHAR(50)= NULL
	SELECT @PartNumberStrip = PartNumberStrip + '%' FROM Items WHERE ItemID = @ItemID;

	WITH Main_Temp AS (
		SELECT  ql.QuoteLineID,
				q.QuoteID,
				q.VersionID,
				q.AccountID,				
				a.AccountName,
				c.ContactID,
				c.FirstName,
				c.LastName,
				q.OrganizationID,
				o.[Name] 'OrgName',
				q.StatusID,
				s.StatusName,
				q.SentDate,
				q.Created 'CreatedDate',
				ql.PartNumber,
				ql.Manufacturer,				
				ql.Qty,
				ql.Price,
				ql.Cost,
				((ql.Qty * ql.Price) - (ql.Qty * ql.Cost)) 'GP',
				CASE WHEN ql.Qty * ql.Price <> 0 THEN ((ql.Qty * ql.Price) - (ql.Qty * ql.Cost)) / ((ql.Qty * ql.Price)) ELSE 0 END 'GPM',
				ql.DateCode,
				ql.PackagingID,
				p.PackagingName,
				dbo.fnGetObjectOwners(q.QuoteID, 19) 'Owners',
				CASE WHEN asec.AccountID IS NULL THEN 0 ELSE 1 END 'CanViewAccount',
				CASE WHEN csec.ContactID IS NULL THEN 0 ELSE 1 END 'CanViewContact',
				CASE WHEN qsec.QuoteID IS NULL THEN 0 ELSE 1 END 'CanViewQuote'
		FROM vwQuoteLines ql
		INNER JOIN vwQuotes q ON ql.QuoteID = q.QuoteID AND ql.QuoteVersionID = q.VersionID AND ISNULL(q.SentDate, q.Created) BETWEEN @StartDate AND @EndDate
		INNER JOIN Accounts a ON q.AccountID = a.AccountID
		INNER JOIN Organizations o ON q.OrganizationID = o.OrganizationID
		LEFT OUTER JOIN Contacts c ON q.ContactID = c.ContactID
		LEFT OUTER JOIN lkpStatuses s ON q.StatusID = s.StatusID
		LEFT OUTER JOIN (SELECT DISTINCT AccountID FROM @AccountSec) asec ON q.AccountID = asec.AccountID
		LEFT OUTER JOIN (SELECT DISTINCT ContactID FROM @ContactSec) csec ON q.ContactID = csec.ContactID
		LEFT OUTER JOIN (SELECT DISTINCT QuoteID FROM @QuoteSec) qsec ON q.QuoteID = qsec.QuoteID
		LEFT OUTER JOIN codes.lkpPackaging p ON ql.PackagingID = p.PackagingID		
		WHERE (ql.ItemID = @ItemID OR ql.PartNumberStrip LIKE ISNULL(@PartNumberStrip,'%'))
		AND a.AccountID = ISNULL(@AccountID, a.AccountID)
		AND ISNULL(c.ContactID, 0) = COALESCE(@ContactID, c.ContactID, 0)
	), 
	Count_Temp AS(
		SELECT COUNT(*) AS TotalRowCount
		From Main_Temp)

	SELECT *
	FROM Main_Temp,Count_Temp
	ORDER BY
		--Ascending
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.SentDate
				WHEN @SortBy = 'SentDate' THEN Main_Temp.SentDate
				WHEN @SortBy = 'CreatedDate' THEN Main_Temp.CreatedDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName				
				WHEN @SortBy = 'OrgName' THEN Main_Temp.OrgName
				WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
				WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
				WHEN @SortBy = 'Owners' THEN Main_Temp.Owners
				WHEN @SortBy = 'StatusName' THEN Main_Temp.StatusName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 				
				WHEN @SortBy = 'Qty' THEN Main_Temp.Qty
				WHEN @SortBy = 'Price' THEN Main_Temp.Price
				WHEN @SortBy = 'Cost' THEN Main_Temp.Cost
				WHEN @SortBy = 'GPM' THEN Main_Temp.GPM
				WHEN @SortBy = 'GP'  THEN Main_Temp.GP
				WHEN @SortBy = 'QuoteID' THEN Main_Temp.QuoteID
			END
		END ASC,
		--Descending
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.SentDate
				WHEN @SortBy = 'SentDate' THEN Main_Temp.SentDate
				WHEN @SortBy = 'CreatedDate' THEN Main_Temp.CreatedDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName				
				WHEN @SortBy = 'OrgName' THEN Main_Temp.OrgName
				WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
				WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
				WHEN @SortBy = 'Owners' THEN Main_Temp.Owners
				WHEN @SortBy = 'StatusName' THEN Main_Temp.StatusName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 				
				WHEN @SortBy = 'Qty' THEN Main_Temp.Qty
				WHEN @SortBy = 'Price' THEN Main_Temp.Price
				WHEN @SortBy = 'Cost' THEN Main_Temp.Cost
				WHEN @SortBy = 'GPM' THEN Main_Temp.GPM
				WHEN @SortBy = 'GP'  THEN Main_Temp.GP
				WHEN @SortBy = 'QuoteID' THEN Main_Temp.QuoteID
			END
		END DESC
		
		OFFSET @RowOffset ROWS
		FETCH NEXT NULLIF(@RowLimit,0) ROWS ONLY
END