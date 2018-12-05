/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Inserts or updates a SalesOrderExtra on a SalesOrder
   Usage:		EXEC uspSalesOrderExtrasGet @SalesOrderID = 100007, @SOVersionID = 2	
   Return Codes:
   Revision History:
			2017.08.22  BZ  Add comments
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspSalesOrderExtrasGet]
	@SalesOrderID INT = NULL,
	@SOVersionID INT = NULL,
	@SOExtraID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
			soe.SOExtraID,			
			soe.LineNum,
			soe.RefLineNum,
			soe.ItemExtraID,
			e.ExtraName,
			e.ExtraDescription,
			soe.Note,
			soe.Qty,
			soe.Price,
			soe.Cost,
			CASE WHEN (Qty * Price) <> 0 THEN ((Qty * Price) - (Qty * Cost)) / (Qty * Price) ELSE 0 END 'GPM',
			soe.StatusID,
			s.StatusName,
			0 'CommentCount', --Placeholder
			soe.PrintOnSO,
			COUNT(*) OVER() AS 'TotalRows',
			dbo.fnGetCommentsCount(soe.SOExtraID, @CommentTypeID) 'Comments'
	FROM SalesOrderExtras soe
	  LEFT OUTER JOIN lkpStatuses s ON soe.StatusID = s.StatusID
	  LEFT OUTER JOIN ItemExtras e ON soe.ItemExtraID = e.ItemExtraID
	WHERE (soe.SOExtraID = @SOExtraID OR (soe.SalesOrderID = @SalesOrderID AND soe.SOVersionID = @SOVersionID))
	  AND soe.IsDeleted = 0
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN soe.LineNum
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN soe.Qty
				WHEN @SortBy = 'Price' THEN soe.Price
				WHEN @SortBy = 'Cost' THEN soe.Cost
				WHEN @SortBy = 'RefLineNum' THEN soe.RefLineNum
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
				WHEN ISNULL(@SortBy, '') = '' THEN soe.LineNum
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN soe.Qty
				WHEN @SortBy = 'Price' THEN soe.Price
				WHEN @SortBy = 'Cost' THEN soe.Cost
				WHEN @SortBy = 'RefLineNum' THEN soe.RefLineNum
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