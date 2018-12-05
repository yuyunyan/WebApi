-- =============================================
-- Author:		Berry
-- Create date: 2017.08.04
-- Description:	Return list of Purchase Order Extras for given PurchaseOrderId and VersionID
-- Revision History:
--			2017.08.22 BZ Add Comments
-- =============================================
CREATE PROCEDURE [dbo].[uspPurchaseOrderExtrasGet]
	@PurchaseOrderID INT = NULL,
	@POVersionID INT = NULL,
	@POExtraID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = 0
AS
BEGIN
	SELECT 
			poe.POExtraID,			
			poe.LineNum,
			poe.RefLineNum,
			poe.ItemExtraID,
			e.ExtraName,
			e.ExtraDescription,
			poe.Note,
			poe.Qty,
			poe.Cost,
			poe.StatusID,
			s.StatusName,
			poe.PrintOnPO,
			COUNT(*) OVER() AS 'TotalRows',
			dbo.fnGetCommentsCount(poe.POExtraID, @CommentTypeID) 'Comments'
	FROM PurchaseOrderExtras poe
	  LEFT OUTER JOIN lkpStatuses s ON poe.StatusID = s.StatusID
	  LEFT OUTER JOIN ItemExtras e ON poe.ItemExtraID = e.ItemExtraID
	WHERE ((poe.PurchaseOrderID = @PurchaseOrderID AND poe.POVersionID = @POVersionID) OR poe.POExtraID = @POExtraID)
	  AND poe.IsDeleted = 0
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN poe.LineNum
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN poe.Qty
				WHEN @SortBy = 'Cost' THEN poe.Cost
				WHEN @SortBy = 'RefLineNum' THEN poe.RefLineNum
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ExtraName' THEN e.ExtraName
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN poe.LineNum
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN poe.Qty
				WHEN @SortBy = 'Cost' THEN poe.Cost
				WHEN @SortBy = 'RefLineNum' THEN poe.RefLineNum
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
