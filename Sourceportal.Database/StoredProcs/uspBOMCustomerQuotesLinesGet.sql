/* =============================================
   Author:			Corey Tyrrell
   Create date:		2017.09.25
   Description:		Retrieves Customer Quotes lines
   Usage:			EXEC uspBOMCustomerQuotesLinesGet @SearchID = 1286 
					EXEC uspBOMCustomerQuotesLinesGet @SearchID = 1286 

   Revision History:
		2017.10.25	AR	Added BOM Columns
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMCustomerQuotesLinesGet]
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
	SELECT R.RecordID QuoteID,
			R.Created QuoteDate,
			R.AccountID,
			R.AccountName Customer,
			R.ContactID,
			R.ContactName Contact,
			R.PartNumber,
			R.Manufacturer Manufacturer,
			R.Qty,
			R.TargetPrice,
			R.OwnerName Owners,			
			R.CustomerPartNumber CustomerPartNumber,
			R.DateCode,
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
	AND R.ResultType = 'C'
	
	ORDER BY
	--Int Type ASC
	CASE WHEN @DescSort = 0 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN AccountID
			WHEN @SortBy = 'ContactID' THEN ContactID
			WHEN @SortBy = 'QuoteID' THEN RecordID
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
			WHEN @SortBy = 'ContactName' THEN ContactName
			WHEN @SortBy = 'SalesRep' THEN OwnerName
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END ASC,
		--Int Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN ISNULL(@SortBy, '') = '' THEN RecordID
			WHEN @SortBy = 'AccountID' THEN AccountID
			WHEN @SortBy = 'QuoteID' THEN RecordID
			WHEN @SortBy = 'Qty' THEN Qty
			WHEN @SortBy = 'TargetPrice' THEN TargetPrice
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
			WHEN @SortBy = 'TargetPrice' THEN TargetPrice
		END
	END DESC,
	--Char Type DESC
	CASE WHEN @DescSort = 1 THEN
		CASE 
			WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			WHEN @SortBy = 'PartNumber' THEN PartNumber			
			WHEN @SortBy = 'IntPartNumber' THEN CustomerPartNumber
			WHEN @SortBy = 'AccountName' THEN AccountName
			WHEN @SortBy = 'ContactName' THEN ContactName
			WHEN @SortBy = 'SalesRep' THEN OwnerName
			WHEN @SortBy = 'DateCode' THEN DateCode
		END
	END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END