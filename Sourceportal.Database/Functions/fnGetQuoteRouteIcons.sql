CREATE FUNCTION [dbo].[fnGetQuoteRouteIcons]
(
	@QuoteLineID INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN	
	RETURN (	
	SELECT	r.QuoteLineID,
		r.RouteStatusID,
		s.StatusName,
		s.Icon,
		s.IconColor,
		u.FirstName + ' ' + u.LastName 'BuyerName'
	FROM mapBuyerQuoteRoutes r
	INNER JOIN lkpRouteStatuses s ON r.RouteStatusID = s.RouteStatusID
	INNER JOIN Users u ON r.UserID = u.UserID
	WHERE r.QuoteLineID = @QuoteLineID AND r.IsDeleted = 0
	FOR JSON PATH)
END