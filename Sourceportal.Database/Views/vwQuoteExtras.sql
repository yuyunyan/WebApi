CREATE VIEW [dbo].[vwQuoteExtras] AS
	
	SELECT qe.* 
	FROM QuoteExtras qe
	INNER JOIN vwQuotes vq ON qe.QuoteID = vq.QuoteID AND qe.QuoteVersionID = vq.VersionID
	WHERE qe.IsDeleted = 0