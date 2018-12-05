/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.04.09
   Description:	Updates SalesOrderLine routes based on buyer specialties
   Usage:		EXEC uspSalesOrderLinesAutoRoute @SalesOrderLinesJSON = '[{"SOLineID":1143}, {"SOLineID":1139}]', @UserID = 0		
   Return Codes:
			-1 Missing UserID
			-2 Missing JSON list of SO Lines to be routed
   Revision History:

   ============================================= */
   
CREATE PROCEDURE [dbo].[uspSalesOrderLinesAutoRoute]
	@SalesOrderLinesJSON VARCHAR(MAX),
	@UserID INT
AS	
BEGIN
	SET NOCOUNT ON;
	IF ISNULL(@SalesOrderLinesJSON, '') = ''
		RETURN -2

	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	
	DECLARE @ToRoute TABLE (SOLineID INT, ItemID INT, MfrID INT, CommodityID INT)
	DECLARE @Routes TABLE (UserID INT, SOLineID INT)
	
	--Create a temp table of all the SalesOrder lines needing to be routed
	INSERT INTO @ToRoute
	SELECT sol.SOLineID, sol.ItemID, i.MfrID, i.CommodityID
	FROM vwSalesOrderLines sol
	INNER JOIN OPENJSON(@SalesOrderLinesJSON) WITH (SOLineID INT) AS jsol ON sol.SOLineID = jsol.SOLineID
	LEFT OUTER JOIN Items i ON sol.ItemID = i.ItemID
	LEFT OUTER JOIN Manufacturers m ON i.MfrID = m.MfrID

	--Find all buyers who specialize in the commodities
	INSERT INTO @Routes
	SELECT bs.UserID, sol.SOLineID
	FROM @ToRoute sol
	INNER JOIN mapBuyerSpecialties bs 
		ON bs.ObjectTypeID = 101 --Commodity ObjectType
		AND bs.ObjectID = sol.CommodityID
		AND bs.IsDeleted = 0
	UNION
	--Find all the buyers who specialize in the manufacturers	
	SELECT bs.UserID, sol.SOLineID
	FROM @ToRoute sol
	INNER JOIN mapBuyerSpecialties bs 
		ON bs.ObjectTypeID = 102 --Manufacturer ObjectType
		AND bs.ObjectID = sol.MfrID
		AND bs.IsDeleted = 0

	--Merge the routes into the mapBuyerSORoutes table
	MERGE mapBuyerSORoutes AS r
	USING (SELECT UserID, SOLineID FROM @Routes) AS t ON (r.SOLineID = t.SOLineID AND r.UserID = t.UserID)
	WHEN NOT MATCHED
		THEN INSERT (UserID, SOLineID, CreatedBy)
		VALUES (t.UserID, t.SOLineID, @UserID)
	WHEN MATCHED
		THEN UPDATE SET r.IsDeleted = 0, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE()
	WHEN NOT MATCHED BY SOURCE AND SOLineID IN(SELECT DISTINCT SOLineID FROM @Routes)
		THEN UPDATE SET r.IsDeleted = 1, r.ModifiedBy = @UserID, r.Modified = GETUTCDATE();
END