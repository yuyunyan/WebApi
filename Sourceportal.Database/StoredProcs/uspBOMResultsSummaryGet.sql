/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.09.25
   Description:	Returns the search results summary for a given SearchID
   Usage:		EXEC uspBOMResultsSummaryGet @SearchID = 52				
   Return Codes:
				-1 SearchID is required				
   Revision History:	
   ============================================= */

CREATE PROCEDURE [dbo].[uspBOMResultsSummaryGet]
	@SearchID INT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@SearchID , 0) = 0
		RETURN -1;
	
	WITH Main_CTE AS(
	SELECT	ItemID,
		PartNumber,
		Manufacturer,
		SUM(CASE WHEN ResultType = 'R' THEN 1 ELSE 0 END) 'CustomerRFQ',
		SUM(CASE WHEN ResultType = 'Q' THEN 1 ELSE 0 END) 'VendorQuotes',
		SUM(CASE WHEN ResultType = 'C' THEN 1 ELSE 0 END) 'CustomerQuotes',
		SUM(CASE WHEN ResultType = 'O' THEN 1 ELSE 0 END) 'OutsideOffers',
		SUM(CASE WHEN ResultType = 'P' THEN 1 ELSE 0 END) 'PurchaseOrders',
		SUM(CASE WHEN ResultType = 'S' THEN 1 ELSE 0 END) 'SalesOrders',
		SUM(CASE WHEN ResultType = 'I' THEN 1 ELSE 0 END) 'Inventory',
		SUM(CASE WHEN ResultType = 'B' THEN 1 ELSE 0 END) 'BOM'
	FROM SearchResults
	WHERE SearchID = @SearchID
	GROUP BY ItemID, PartNumber, Manufacturer),

	Count_CTE AS (
		SELECT COUNT(*) AS [RowCount]
		FROM Main_CTE)

	SELECT * FROM Main_CTE, Count_CTE
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, 'PartNumber') = 'PartNumber' THEN PartNumber
				WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, 'PartNumber') = 'PartNumber' THEN PartNumber
				WHEN @SortBy = 'Manufacturer' THEN Manufacturer
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END

