/* =============================================
   Author:			Corey Tyrrell
   Create date:		2017.09.25
   Description:		Retrieves BOM/EMS Record lines
   Usage:			EXEC uspBOMRecordLinesGet @SearchID = 1150 
   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMRecordLinesGet]
(
	@SearchID INT,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT R.RecordID ItemListID,
			R.Created BOMDate,
			R.AccountID,
			R.AccountName Customer,
			R.PartNumber,
			R.Manufacturer Manufacturer,
			R.Qty,
			R.TargetPrice,
			R.OwnerName,
			R.CustomerPartNumber CustomerPartNum,
			R.PriceDelta,
			R.Potential,
			R.BOMPrice,
			R.BOMPartNumber,
			R.BOMIntPartNumber,
			R.BOMMfg,
			R.BOMQty,
			R.ResultType,
			COUNT(*) OVER() AS 'TotalRows'
	FROM SearchResults R
	WHERE SearchID = @SearchID
	AND R.ResultType = 'B'
	
	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN AccountID
			WHEN @SortBy = 'SourceID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'TargetPrice' THEN TargetPrice
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
			WHEN @SortBy = 'TargetPrice' THEN TargetPrice
		END
	END ASC,
	--Char Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			WHEN @SortBy = 'PartNumber' THEN PartNumber
			WHEN @SortBy = 'IntPartNumber' THEN CustomerPartNumber
			WHEN @SortBy = 'AccountName' THEN AccountName
		END
	END ASC,
		--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN AccountID
			WHEN @SortBy = 'SourceID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'TargetPrice' THEN TargetPrice
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
			WHEN @SortBy = 'IntPartNumber' THEN CustomerPartNumber
			WHEN @SortBy = 'AccountName' THEN AccountName
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END