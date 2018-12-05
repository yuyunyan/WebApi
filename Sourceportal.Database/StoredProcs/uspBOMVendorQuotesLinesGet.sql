/* =============================================
   Author:			Corey Tyrrell
   Create date:		2017.09.25
   Description:		Retrieves Vendor Quotes lines
   Usage:			EXEC uspBOMVendorQuotesLinesGet @SearchID = 48 
   Revision History:
		2017.10.25.17	AR	Added BOM Columns
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMVendorQuotesLinesGet]
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
			R.Note,
			R.SPQ,
			R.MOQ,
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
	AND R.ResultType = 'Q'
	
	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN R.AccountID
			WHEN @SortBy = 'SourceID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'Cost' THEN Cost
			WHEN @SortBy = 'LeadTimeDays' THEN LeadTimeDays
		END
	END ASC,	

	--Date Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN @SortBy = 'Created' THEN R.Created
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
			WHEN @SortBy = 'AccountName' THEN R.AccountName
			WHEN @SortBy = 'Buyer' THEN Buyer
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END ASC,
		--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN R.AccountID
			WHEN @SortBy = 'SourceID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'Cost' THEN Cost
			WHEN @SortBy = 'LeadTimeDays' THEN LeadTimeDays
		END
	END DESC,

	--Date Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Created' THEN R.Created
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
			WHEN @SortBy = 'AccountName' THEN R.AccountName
			WHEN @SortBy = 'Buyer' THEN Buyer
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END