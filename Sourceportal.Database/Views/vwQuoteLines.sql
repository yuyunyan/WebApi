/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.19
   Description:	Limits the view of QuoteLines to only those on the latest version of a Quote
   Revision History:
	
   ============================================= */

CREATE VIEW [dbo].[vwQuoteLines] AS
	
	SELECT ql.* 
	FROM QuoteLines ql
	INNER JOIN vwQuotes vq ON ql.QuoteID = vq.QuoteID AND ql.QuoteVersionID = vq.VersionID
	WHERE ql.IsDeleted = 0