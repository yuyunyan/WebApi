/* =============================================
   Author:				Berry, Zhong
   Create date:			2018.03.06
   Description:			Return list of other users that quote line routed to
   Revision History:
	2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name

   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteLineRouteBuyersGet]
	@UserID INT = NULL,
	@QuoteLineID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Distinct (USR.FirstName + ' ' + USR.LastName + ' [' + RS.StatusName + ']') 'BuyerName'
	FROM mapBuyerQuoteRoutes MBR
		LEFT JOIN Users USR ON USR.UserID = MBR.UserID AND USR.IsEnabled = 1
		LEFT JOIN lkpRouteStatuses RS ON RS.RouteStatusID = MBR.RouteStatusID AND RS.IsDeleted = 0
	WHERE MBR.UserID != @UserID AND MBR.QuoteLineID = @QuoteLineID
END
GO
