/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.02.16
   Description:	Updates QuoteLine routes based on buyer specialties
   Usage:		EXEC uspQuoteLinesAutoRoute @QuoteLinesJSON = '[{"QuoteLineID":16}, {"QuoteLineID":88}]', @UserID = 0		
   Return Codes:
			-1 Missing UserID
			-2 Missing JSON list of Quote Lines to be routed
   Revision History:
			2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name
   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteLinesAutoRoute]
	@QuoteLinesJSON VARCHAR(MAX),
	@UserID INT
AS	
BEGIN
	IF ISNULL(@QuoteLinesJSON, '') = ''
		RETURN -2

	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	
	DECLARE @ToRoute TABLE (QuoteLineID INT, ItemID INT, MfrID INT, CommodityID INT)
	DECLARE @Routes TABLE (UserID INT, QuoteLineID INT)
	
	--Create a temp table of all the quote lines needing to be routed
	INSERT INTO @ToRoute
	SELECT ql.QuoteLineID, ql.ItemID, i.MfrID, ql.CommodityID
	FROM vwQuoteLines ql
	INNER JOIN OPENJSON(@QuoteLinesJSON) WITH (QuoteLineID INT) AS jql ON ql.QuoteLineID = jql.QuoteLineID
	LEFT OUTER JOIN Items i ON ql.ItemID = i.ItemID
	LEFT OUTER JOIN Manufacturers m ON i.MfrID = m.MfrID

	--Find all buyers who specialize in the commodities
	--Works for quote lines with or without an ItemID
	INSERT INTO @Routes
	SELECT bs.UserID, q.QuoteLineID
	FROM @ToRoute q
	INNER JOIN mapBuyerSpecialties bs 
		ON bs.ObjectTypeID = 101 --Commodity ObjectType
		AND bs.ObjectID = q.CommodityID
		AND bs.IsDeleted = 0
	UNION
	--Find all the buyers who specialize in the manufacturers
	--This section only works if the quote line has an ItemID
	SELECT bs.UserID, q.QuoteLineID
	FROM @ToRoute q
	INNER JOIN mapBuyerSpecialties bs 
		ON bs.ObjectTypeID = 102 --Manufacturer ObjectType
		AND bs.ObjectID = q.MfrID
		AND bs.IsDeleted = 0

	--Merge the routes into the mapBuyerRoutes table
	MERGE mapBuyerQuoteRoutes AS r
	USING (SELECT UserID, QuoteLineID FROM @Routes) AS t ON (r.QuoteLineID = t.QuoteLineID AND r.UserID = t.UserID)
	WHEN NOT MATCHED
		THEN INSERT (UserID, QuoteLineID, RouteStatusID, CreatedBy)
		VALUES (t.UserID, t.QuoteLineID, 1, @UserID)
	WHEN MATCHED
		THEN UPDATE SET r.IsDeleted = 0, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE()
	WHEN NOT MATCHED BY SOURCE AND QuoteLineID IN(SELECT DISTINCT QuoteLineID FROM @Routes)
		THEN UPDATE SET r.IsDeleted = 1, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE();

	UPDATE QuoteLines
	SET StatusID = (SELECT ConfigValue FROM lkpConfigVariables WHERE ConfigName = 'QuoteLineRoutedToBuyerStatus')
	FROM QuoteLines QL
		INNER JOIN @ToRoute t ON QL.QuoteLineID = t.QuoteLineID
	WHERE QL.IsDeleted = 0

END
