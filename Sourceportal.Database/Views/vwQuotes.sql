

CREATE VIEW [dbo].[vwQuotes] AS 

	WITH q AS
		(
			SELECT *,
				ROW_NUMBER() OVER (PARTITION BY QuoteID ORDER BY VersionID DESC) AS vn
			FROM Quotes
			WHERE IsDeleted = 0			  
		)
	SELECT * FROM q WHERE vn = 1
