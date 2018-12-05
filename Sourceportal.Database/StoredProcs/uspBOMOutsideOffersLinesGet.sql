/* =============================================
   Author:			Corey Tyrrell
   Create date:		2017.09.25
   Description:		Retrieves Outside Offer lines
   Usage:			EXEC uspBOMOutsideOffersLinesGet @SearchID = 50 , @PartNumber='bav'
   
   Revision History:
		2017.09.15	AR	Added SearchResults join
		2017.10.25	AR	Added BOM Columns
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMOutsideOffersLinesGet]
(
	@SearchID INT,
	--@PartNumber VARCHAR(128) = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT R.RecordID SourceID,
			R.Created,
			R.AccountID,
			R.AccountName,
			R.PartNumber,
			R.Manufacturer Manufacturer,
			R.Qty,
			R.Cost,
			R.Buyer,
			R.DateCode,
			R.LeadTimeDays,
			R.PriceDelta,
			R.Potential,
			R.BOMPrice,
			R.BOMPartNumber,
			R.BOMIntPartNumber,
			R.BOMMfg,
			R.BOMQty,
			COUNT(*) OVER() AS 'TotalRows'
	FROM SearchResults R
	WHERE SearchID = @SearchID
--	AND (R.PartNumber LIKE ISNULL(NULLIF(@PartNumber,''), PartNumber) + '%')
	AND R.ResultType = 'O'
	
	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN AccountID
			WHEN @SortBy = 'SourceID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'Cost' THEN Cost
			WHEN @SortBy = 'LeadTimeDays' THEN LeadTimeDays
		END
	END ASC,	

	--Date Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Created' THEN Created
		END
	END ASC,
	--Float Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Cost' THEN Cost
		END
	END ASC,
	--Char Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'AccountName' THEN AccountName
			WHEN @SortBy = 'Buyer' THEN Buyer
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END ASC,
		--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN AccountID
			WHEN @SortBy = 'SourceID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'Cost' THEN Cost
			WHEN @SortBy = 'LeadTimeDays' THEN LeadTimeDays
		END
	END DESC,

	--Date Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Created' THEN Created
		END
	END DESC,
	--Float Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Cost' THEN Cost
		END
	END DESC,
	--Char Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'AccountName' THEN AccountName
			WHEN @SortBy = 'Buyer' THEN Buyer
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END