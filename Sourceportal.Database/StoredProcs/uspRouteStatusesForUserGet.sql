/* =============================================
   Author:				Berry, Zhong
   Create date:			2018.03.05
   Description:			Return RouteStatus for user
   Revision History:
  			2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name
			2018.04.16	BZ	Added owner join conditions to show number of quoteLines correctly
   ============================================= */
CREATE PROCEDURE [dbo].[uspRouteStatusesForUserGet]
	@UserID INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		RS.RouteStatusID,
		RS.StatusName,
		RS.IsDefault,
		RS.IsComplete,
		(SELECT
			COUNT(*)
		 FROM mapBuyerQuoteRoutes MBR
			INNER JOIN lkpRouteStatuses lrs on lrs.RouteStatusID = MBR.RouteStatusID AND lrs.IsDeleted = 0
			INNER JOIN vwQuoteLines ql ON MBR.QuoteLineID = ql.QuoteLineID
			INNER JOIN Quotes q ON ql.QuoteID = q.QuoteID AND ql.QuoteVersionID = q.VersionID
			INNER JOIN (SELECT OwnerID, ObjectID, IsGroup,
						ROW_NUMBER() OVER (PARTITION BY ObjectID, ObjectTypeID ORDER BY [Percent] DESC) AS a
						FROM mapOwnership
						WHERE IsDeleted = 0 AND ObjectTypeID = 19 --Quote ObjectTypeID
						) o ON q.QuoteID = o.ObjectID AND o.a = 1
			INNER JOIN Users u ON o.OwnerID = u.UserID
		 WHERE lrs.IsComplete = 0 AND lrs.RouteStatusID = RS.RouteStatusID AND MBR.UserID = @UserID AND MBR.IsDeleted = 0
		 Group BY MBR.RouteStatusID) 'CountQuoteLines'
	FROM lkpRouteStatuses RS
	WHERE RS.IsDeleted = 0
END
GO
