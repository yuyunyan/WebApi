/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.30
   Description:	Inserts or updates a Quote Extra on a Quote
   Usage:		EXEC uspQuoteExtrasGet @QuoteID = 100001, @QuoteVersionID = 3	
   Return Codes:
   Revision History:
			2017.07.19  NA  Implemented vwQuoteExtras
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteExtrasGet]
	@QuoteID INT = NULL,
	@QuoteVersionID INT = NULL,
	@QuoteExtraID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
			qe.QuoteExtraID,
			qe.LineNum,
			qe.RefLineNum,
			qe.ItemExtraID,
			e.ExtraName,
			e.ExtraDescription,
			qe.Note,
			qe.Qty,
			qe.Price,
			qe.Cost,
			CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END 'GPM',
			qe.StatusID,
			s.StatusName,
			0 'CommentCount', --Placeholder
			qe.PrintOnQuote,
			COUNT(*) OVER() AS 'TotalRows',
			dbo.fnGetCommentsCount(qe.QuoteExtraID, @CommentTypeID) 'Comments'
	FROM vwQuoteExtras qe
	  LEFT OUTER JOIN lkpStatuses s ON qe.StatusID = s.StatusID
	  LEFT OUTER JOIN ItemExtras e ON qe.ItemExtraID = e.ItemExtraID
	WHERE (qe.QuoteExtraID = @QuoteExtraID OR (qe.QuoteID = @QuoteID AND qe.QuoteVersionID = @QuoteVersionID))
		and qe.IsDeleted = 0	  
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN qe.LineNum
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN qe.Qty
				WHEN @SortBy = 'Price' THEN qe.Price
				WHEN @SortBy = 'Cost' THEN qe.Cost
				WHEN @SortBy = 'RefLineNum' THEN qe.RefLineNum
				WHEN @SortBy = 'GPM' THEN CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ExtraName' THEN e.ExtraName
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN qe.LineNum
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN qe.Qty
				WHEN @SortBy = 'Price' THEN qe.Price
				WHEN @SortBy = 'Cost' THEN qe.Cost
				WHEN @SortBy = 'RefLineNum' THEN qe.RefLineNum
				WHEN @SortBy = 'GPM' THEN CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ExtraName' THEN e.ExtraName			
			END
		END DESC	
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END