/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Gets the Quote Lines for the Sourcing grid by RouteStatus				
   Usage: EXEC uspQuoteLinesForSourcingGet @RouteStatusID = 1, @UserID = 64
   Revision History:
			2017.08.22	BZ	Add Comments
			2017.10.05	AR	Added quoteID to sort
			2017.11.28	BZ	Added Security
			2018.01.30	RV	Added RfqCount , Quote Type , filter logic
			2018.02.21	NA	Updated to use BuyerRoutes
			2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name
   Return Codes:

   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteLinesForSourcingGet]
	@RouteStatusID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@FilterBy NVARCHAR(25) = NULL,
	@FilterText VARCHAR(256) = '',
	@CommentTypeID INT = 0,
	@UserID INT = 64
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Sec TABLE (QuoteID INT, RoleID INT)
	INSERT @Sec EXECUTE uspQuoteSecurityGet @UserID = @UserID;

	--Find the source counts for each unique PartNumberStrip that are on open quotes
	DECLARE @SourceCounts TABLE (PartNumberStrip VARCHAR(32), SourcesCount INT)
	INSERT INTO @SourceCounts
		SELECT ql.PartNumberStrip, COUNT(s.SourceID) 'SourcesCount'
		FROM
		(SELECT DISTINCT PartNumberStrip FROM vwQuoteLines) ql
		LEFT OUTER JOIN Sources s ON s.PartNumberStrip LIKE ql.PartNumberStrip + '%' AND s.IsDeleted = 0
		GROUP BY ql.PartNumberStrip
	--Find the RFQ lines counts for each unique PartNumberStrip that have isCanceled = 0 or IsComplete = 0
	DECLARE @RfqCounts TABLE (PartNumberStrip VARCHAR(32), RfqCount INT)
	INSERT INTO @RfqCounts
		SELECT PartNumberStrip , COUNT(v.VRFQLineID) 'RFQCount' 
		FROM VendorRFQLines v 
			INNER JOIN lkpStatuses s ON v.StatusID = s.StatusID AND s.ObjectTypeID = 28 AND s.IsComplete = 0 AND s.IsCanceled = 0
		WHERE v.IsDeleted = 0
		GROUP BY v.PartNumberStrip
	SELECT ql.QuoteLineID, 
		ql.QuoteID, 
		ql.QuoteVersionID, 
		q.AccountID, 
		a.AccountName,
		ql.LineNum,
		ql.PartNumber,
		ql.PartNumberStrip,
		ql.Manufacturer,
		ql.CommodityID,
		ic.CommodityName,
		br.RouteStatusID,
		rs.StatusName,
		ql.Qty,
		ql.PackagingID,
		p.PackagingName,
		ql.DateCode,
		ql.Cost,
		ql.Price,
		ql.ItemID,
		ql.ItemListLineID,
		ql.DueDate,
		ql.ShipDate,
		ql.CustomerLine,
		ql.TargetDateCode ,
		ql.TargetPrice,		
		dbo.fnGetObjectOwners(q.QuoteID, 19) Owners, --19 = Quote (lkpObjectTypes)
		sc.SourcesCount 'SourcesCount', --Placeholder
		 rfq.RfqCount 'RfqCount', --Placeholder
		COUNT(*) OVER() AS 'TotalRows',
		dbo.fnGetCommentsCount(ql.QuoteLineID, @CommentTypeID) 'Comments',
		qt.TypeName
	FROM mapBuyerQuoteRoutes br 
		INNER JOIN vwQuoteLines ql ON br.QuoteLineID = ql.QuoteLineID
		INNER JOIN Quotes q ON ql.QuoteID = q.QuoteID AND ql.QuoteVersionID = q.VersionID
		INNER JOIN lkpQuoteTypes qt on q.QuoteTypeID = qt.QuoteTypeID 
		INNER JOIN (SELECT DISTINCT QuoteID FROM @Sec) sec ON q.QuoteID = sec.QuoteID
		INNER JOIN Accounts a ON q.AccountID = a.AccountID
		INNER JOIN lkpItemCommodities ic ON ql.CommodityID = ic.CommodityID
		INNER JOIN (SELECT OwnerID, ObjectID, IsGroup,
					ROW_NUMBER() OVER (PARTITION BY ObjectID, ObjectTypeID ORDER BY [Percent] DESC) AS a
					FROM mapOwnership
					WHERE IsDeleted = 0 AND ObjectTypeID = 19 --Quote ObjectTypeID
					) o ON q.QuoteID = o.ObjectID AND o.a = 1
		INNER JOIN Users u ON o.OwnerID = u.UserID
		INNER JOIN lkpRouteStatuses rs ON br.RouteStatusID = rs.RouteStatusID
		LEFT OUTER JOIN codes.lkpPackaging p ON ql.PackagingID = p.PackagingID
		LEFT OUTER JOIN @SourceCounts sc ON ql.PartNumberStrip = sc.PartNumberStrip
		LEFT OUTER JOIN @RfqCounts rfq ON ql.PartNumberStrip = rfq.PartNumberStrip 
	WHERE br.UserID = @UserID AND br.IsDeleted = 0 
		AND ISNULL(@RouteStatusID, br.RouteStatusID) = br.RouteStatusID
		AND	(@FilterBy IS NULL 
			OR (
				   (@FilterBy = 'Type' AND qt.TypeName LIKE '%' + ISNULL(@FilterText , '') + '%')
                OR (@FilterBy = 'quoteId' AND ql.QuoteID LIKE ISNULL(@FilterText , '') + '%')
                OR (@FilterBy = 'AccountName' AND a.AccountName LIKE '%' + ISNULL(@FilterText , '') + '%')
				OR (@FilterBy = 'PartNumber' AND ql.PartNumber LIKE '%' + ISNULL(@FilterText , '') + '%')
				OR (@FilterBy = 'Manufacturer' AND ql.Manufacturer LIKE '%' + ISNULL(@FilterText , '') + '%')
				OR (@FilterBy = 'CommodityName' AND ic.CommodityName LIKE '%' + ISNULL(@FilterText , '') + '%')
				OR (@FilterBy = 'Owners' AND dbo.fnGetObjectOwners(q.QuoteID, 19) LIKE '%' + ISNULL(@FilterText , '') + '%')
              ))
	ORDER BY
		-------------Ascending----------------
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN ql.QuoteID
				WHEN @SortBy = 'quoteId' THEN ql.QuoteID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Type' THEN qt.TypeName	
				WHEN @SortBy = 'AccountName' THEN a.AccountName
				WHEN @SortBy = 'PartNumber' THEN ql.PartNumber				
				WHEN @SortBy = 'Manufacturer' THEN ql.Manufacturer
				WHEN @SortBy = 'CommodityName' THEN ic.CommodityName
				WHEN @SortBy = 'Owners' THEN dbo.fnGetObjectOwners(q.QuoteID, 19)
			END
		END ASC,
		-------------Descending----------------
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN ql.QuoteID
				WHEN @SortBy = 'quoteId' THEN ql.QuoteID	
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Type' THEN qt.TypeName	
				WHEN @SortBy = 'AccountName' THEN a.AccountName
				WHEN @SortBy = 'PartNumber' THEN ql.PartNumber				
				WHEN @SortBy = 'Manufacturer' THEN ql.Manufacturer
				WHEN @SortBy = 'CommodityName' THEN ic.CommodityName
				WHEN @SortBy = 'Owners' THEN dbo.fnGetObjectOwners(q.QuoteID, 19)
			END
		END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END
