/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.02.16
   Description:	Routes the given quote lines to the given buyers
   Usage:		EXEC uspQuoteLinesSpecificRoute @QuoteLinesJSON = '[{"QuoteLineID":16}, {"QuoteLineID":88}]', @UsersJSON = '[{"UserID":61},{"UserID":62}]', @UserID = 0		
   Return Codes:
			-1 Missing UserID
			-2 Missing JSON list of Quote Lines to be routed
			-3 Missing JSON list of UserIDs (buyers) the Lines should be routed to
   Revision History:
			2018.03.21	BZ	Added statusID update after routedTo
			2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name
   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteLinesSpecificRoute]
	@UsersJSON VARCHAR(MAX),
	@QuoteLinesJSON VARCHAR(MAX),
	@UserID INT
AS
BEGIN
	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	IF ISNULL(@QuoteLinesJSON, '') = ''
		RETURN -2
	IF ISNULL(@UsersJSON, '') = ''
		RETURN -3

	DECLARE @Temp TABLE (UserID INT, QuoteLineID INT)

	--Create a cross joined list of all quote lines to all buyers
	INSERT @Temp SELECT u.UserID, q.QuoteLineID 
	FROM OPENJSON(@UsersJSON) WITH (UserID INT) AS u
	CROSS JOIN OPENJSON(@QuoteLinesJSON) WITH (QuoteLineID INT) AS q

	--Merge the routes into the mapBuyerRoutes table
	MERGE mapBuyerQuoteRoutes AS r
	USING (SELECT UserID, QuoteLineID FROM @Temp) AS t ON (r.QuoteLineID = t.QuoteLineID AND r.UserID = t.UserID)
	WHEN NOT MATCHED
		THEN INSERT (UserID, QuoteLineID, RouteStatusID, CreatedBy)
		VALUES (t.UserID, t.QuoteLineID, 1, @UserID)
	WHEN MATCHED
		THEN UPDATE SET r.IsDeleted = 0, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE()
	WHEN NOT MATCHED BY SOURCE AND QuoteLineID IN(SELECT DISTINCT QuoteLineID FROM @Temp)
		THEN UPDATE SET r.IsDeleted = 1, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE();

	UPDATE QuoteLines
	SET StatusID = (SELECT ConfigValue FROM lkpConfigVariables WHERE ConfigName = 'QuoteLineRoutedToBuyerStatus')
	FROM QuoteLines QL
		INNER JOIN @Temp t ON QL.QuoteLineID = t.QuoteLineID
	WHERE QL.IsDeleted = 0

END
