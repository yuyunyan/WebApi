/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.04.10
   Description:	Routes the given SalesOrder lines to the given buyers
   Usage:		EXEC uspSalesOrderLinesSpecificRoute @SalesOrderLinesJSON = '[{"SOLineID":1092}, {"SOLineID":1093}, {"SOLineID":1094}]', @UsersJSON = '[{"UserID":64},{"UserID":66}]', @UserID = 64	
   Return Codes:
			-1 Missing UserID
			-2 Missing JSON list of SalesOrder Lines to be routed
			-3 Missing JSON list of UserIDs (buyers) the Lines should be routed to
   Revision History:

   ============================================= */

CREATE PROCEDURE [dbo].[uspSalesOrderLinesSpecificRoute]
	@UsersJSON VARCHAR(MAX),
	@SalesOrderLinesJSON VARCHAR(MAX),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	IF ISNULL(@SalesOrderLinesJSON, '') = ''
		RETURN -2
	IF ISNULL(@UsersJSON, '') = ''
		RETURN -3

	DECLARE @Temp TABLE (UserID INT, SOLineID INT)

	--Create a cross joined list of all SalesOrder lines to all buyers
	INSERT @Temp SELECT u.UserID, sol.SOLineID 
	FROM OPENJSON(@UsersJSON) WITH (UserID INT) AS u
	CROSS JOIN OPENJSON(@SalesOrderLinesJSON) WITH (SOLineID INT) AS sol

	--Merge the routes into the mapBuyerSORoutes table
	MERGE mapBuyerSORoutes AS r
	USING (SELECT UserID, SOLineID FROM @Temp) AS t ON (r.SOLineID = t.SOLineID AND r.UserID = t.UserID)
	WHEN NOT MATCHED
		THEN INSERT (UserID, SOLineID, CreatedBy)
		VALUES (t.UserID, t.SOLineID, @UserID)
	WHEN MATCHED
		THEN UPDATE SET r.IsDeleted = 0, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE()
	WHEN NOT MATCHED BY SOURCE AND SOLineID IN(SELECT DISTINCT SOLineID FROM @Temp)
		THEN UPDATE SET r.IsDeleted = 1, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE();
END