/* =============================================
   Author:				Berry, Zhong
   Create date:			2018.03.5
   Description:			Update mapBuyerRoutes 
   Usage:				EXEC uspMapBuyerRoutesSet @UserID = 68, @RouteStatusID = 4, @QuoteLinesJSON = '[{"QuoteLineID": 1901}]'
   Revision History:
  			2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name
   ============================================= */
CREATE PROCEDURE [dbo].[uspMapBuyerRoutesSet]
	@UserID INT = NULL,
	@QuoteLinesJSON VARCHAR(MAX) = '[]',
	@RouteStatusID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [mapBuyerQuoteRoutes]
	SET RouteStatusID = @RouteStatusID
	FROM [mapBuyerQuoteRoutes] AS MBR
		INNER JOIN OPENJSON(@QuoteLinesJSON) WITH (QuoteLineID INT) AS QL ON MBR.QuoteLineID = QL.QuoteLineID
	WHERE UserID = @UserID

	SELECT @@ROWCOUNT 'RowCount'
END
GO
