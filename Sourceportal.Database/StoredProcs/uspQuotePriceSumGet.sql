/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.02.02
   Description:	Gets the line items cost SUM
   Usage:		EXEC uspQuotePriceSumGet @QuoteID = 100007, @QuoteVersionID = 2

   Revision History:
		2018.02.02	AR	Intitial Deployment
		2018.02.13	AR	Implemented dbo.fnFormatWithCommas()
   Return Codes:

   ============================================= */

CREATE PROCEDURE [dbo].[uspQuotePriceSumGet]
	
	@QuoteID INT = NULL,
	@QuoteVersionID INT = NULL
AS
BEGIN

	SELECT dbo.fnFormatWithCommas(CAST(ROUND(SUM(ql.Qty * ql.Price), 2) AS NUMERIC(12,2)))
		+ ' ' + (SELECT CurrencyID FROM Quotes WHERE QuoteID = @QuoteID AND VersionID = @QuoteVersionID) AS PriceSum	
	FROM QuoteLines ql
	INNER JOIN Quotes Q on Q.QuoteID = ql.QuoteID AND Q.VersionID = @QuoteVersionID
	WHERE ql.QuoteID = @QuoteID AND ql.QuoteVersionID = @QuoteVersionID
	AND ql.IsDeleted = 0	   
	AND ql.IsPrinted = 1

END
