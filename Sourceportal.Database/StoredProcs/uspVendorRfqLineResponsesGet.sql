/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.13
   Description:	Gets the responses for a rfql line 
   Usage: EXEC uspVendorRfqLineResponsesGet @VRfqLineID  = 1

   Revision History:
	2017.09.15	ML	Added SourceID
	2017.09.18	ML	Filter our delted lines
	2018.04.27	AR	Added ItemID and IsNoStock
	2018.05.01	AR	Changed lkpPackaging to outer join
   Return Codes:

   ============================================= */

CREATE PROCEDURE [dbo].[uspVendorRfqLineResponsesGet]
	@VRfqLineID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@SourceID INT = NULL,
	@CommentTypeID INT = NULL
	
AS
BEGIN
WITH Main_CTE AS(
	SELECT
	S.SourceID,  
	RFQL.LineNum, 
	S.Qty AS OfferQty, 
	S.Cost, 
	S.DateCode, 
	S.MOQ, 
	S.SPQ, 
	S.LeadTimeDays,
	S.PackagingID,
	S.PartNumber,
	S.ItemID,
	S.Manufacturer,
	P.PackagingName,
	S.ValidForHours,
	RFQL.HasNoStock,
	S.IsNoStock,
	dbo.fnGetCommentsCount(SJ.CommentUID, @CommentTypeID) 'Comments'
	FROM

	VendorRFQLines RFQL 
	INNER JOIN mapSourcesJoin SJ ON RFQL.VRFQLineID = SJ.ObjectID AND SJ.objecttypeid = 28 AND SJ.IsDeleted = 0
	INNER JOIN Sources S ON SJ.SourceID = S.SourceID AND S.IsDeleted = 0
	LEFT OUTER JOIN codes.lkpPackaging P ON S.PackagingID = P.PackagingID
	
	WHERE RFQL.VRFQLineID = ISNULL(@VRfqLineID,RFQL.VRFQLineID) AND S.SourceID = ISNULL(@SourceID, S.SourceID)

),
	  
	  Count_CTE AS (
		SELECT COUNT(*) AS [RowCount]
		FROM Main_CTE
	  )

	  SELECT *
FROM Main_CTE, Count_CTE
	
	ORDER BY 
		
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.SourceID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'OfferQty' THEN Main_CTE.OfferQty
				WHEN @SortBy = 'Cost' THEN Main_CTE.Cost 
				WHEN @SortBy = 'MOQ' THEN Main_CTE.MOQ
				WHEN @SortBy = 'SPQ' THEN Main_CTE.SPQ 
				WHEN @SortBy = 'LeadTimeDays' THEN Main_CTE.LeadTimeDays 
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'DateCode' THEN Main_CTE.DateCode 
			END
		END ASC,
		
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.SourceID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'OfferQty' THEN Main_CTE.OfferQty
				WHEN @SortBy = 'Cost' THEN Main_CTE.Cost 
				WHEN @SortBy = 'MOQ' THEN Main_CTE.MOQ
				WHEN @SortBy = 'SPQ' THEN Main_CTE.SPQ 
				WHEN @SortBy = 'LeadTimeDays' THEN Main_CTE.LeadTimeDays 
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'DateCode' THEN Main_CTE.DateCode 
			END
		END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY


	IF (@@rowcount = 0)
		RETURN -1

END

